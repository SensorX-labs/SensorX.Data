using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsHandler(
    IQueryBuilder<Product> _productBuilder,
    IQueryBuilder<Category> _categoryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListProductsQuery, Result<ProductOffsetPagedResult>>
{
    public async Task<Result<ProductOffsetPagedResult>> Handle(
        GetPageListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var productQuery = _productBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

            // Get total count before pagination
            var totalCount = await _queryExecutor.CountAsync(productQuery, cancellationToken);

            // Apply ordering and pagination
            var pagedProductBaseQuery = productQuery
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .ApplyOffsetPagination(request);

            // Join to get additional info
            var sourceQuery = from product in pagedProductBaseQuery
                              join category in _categoryBuilder.QueryAsNoTracking
                                  on product.CategoryId equals category.Id into cs
                              from c in cs.DefaultIfEmpty()
                              select new { product, category = c };

            var dtoQuery = sourceQuery.Select(x => new GetPageListProductsResponse(
                x.product.Id.Value,
                x.product.Code.Value,
                x.product.Name,
                x.product.Manufacture,
                x.category != null ? x.category.Name : "",
                x.product.Status,
                x.product.CreatedAt,
                x.product.Images.Select(i => i.ImageUrl).ToList()
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            var result = new ProductOffsetPagedResult
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            };

            return Result<ProductOffsetPagedResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<ProductOffsetPagedResult>.Failure($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
        }
    }
}