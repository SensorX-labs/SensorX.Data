using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsHandler(
    IQueryBuilder<Product> productBuilder,
    IQueryBuilder<Category> categoryBuilder,
    IQueryBuilder<Supplier> supplierBuilder,
    IQueryBuilder<UnitOfQuantity> unitOfQuantityBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetPageListProductsQuery, Result<OffsetPagedResult<GetPageListProductsResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListProductsResponse>>> Handle(
        GetPageListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<Product> query = productBuilder.QueryAsNoTracking;

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status);
            }

            var sourceQuery = from product in query
                              join category in categoryBuilder.QueryAsNoTracking
                                  on product.CategoryId equals category.Id into cs
                              from c in cs.DefaultIfEmpty()
                              join supplier in supplierBuilder.QueryAsNoTracking
                                  on product.SupplierId equals supplier.Id into ss
                              from s in ss.DefaultIfEmpty()
                              join unit in unitOfQuantityBuilder.QueryAsNoTracking
                                  on product.UnitOfQuantityId equals unit.Id into us
                              from u in us.DefaultIfEmpty()
                              select new { product, category = c, supplier = s, unit = u };

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                sourceQuery = sourceQuery.Where(x =>
                    x.product.Name.ToLower().Contains(term) ||
                    x.product.Code.Value.ToLower().Contains(term) ||
                    (x.supplier != null && x.supplier.Name.ToLower().Contains(term)));
            }

            var totalCount = await queryExecutor.CountAsync(sourceQuery, cancellationToken);

            var pagedSourceQuery = sourceQuery
                .OrderByDescending(x => x.product.CreatedAt)
                .ThenByDescending(x => x.product.Id)
                .ApplyOffsetPagination(request);

            var dtoQuery = pagedSourceQuery.Select(x => new GetPageListProductsResponse(
                x.product.Id.Value,
                x.product.Code.Value,
                x.product.Name,
                x.supplier != null ? x.supplier.Name : "",
                x.category != null ? x.category.Name : "",
                x.product.Status,
                x.product.CreatedAt,
                x.product.Images.Select(i => i.ImageUrl).ToList(),
                x.unit != null ? x.unit.Name : ""
            ));

            var items = await queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            return Result<OffsetPagedResult<GetPageListProductsResponse>>.Success(new OffsetPagedResult<GetPageListProductsResponse>
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            });
        }
        catch (Exception ex)
        {
            return Result<OffsetPagedResult<GetPageListProductsResponse>>.Failure($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
        }
    }
}
