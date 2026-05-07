using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsHandler(
    IQueryBuilder<Product> _productBuilder,
    IQueryBuilder<Category> _categoryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListProductsQuery, Result<OffsetPagedResult<GetPageListProductsResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListProductsResponse>>> Handle(
        GetPageListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<Product> query = _productBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status);
            }
            var totalCount = await _queryExecutor.CountAsync(query, cancellationToken);

            var pagedProductBaseQuery = query
                            .OrderByDescending(x => x.CreatedAt)
                            .ThenByDescending(x => x.Id)
                            .ApplyOffsetPagination(request);

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
                x.product.Images.Select(i => i.ImageUrl).ToList(),
                x.product.Unit
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            var result = new OffsetPagedResult<GetPageListProductsResponse>
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            };

            return Result<OffsetPagedResult<GetPageListProductsResponse>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<OffsetPagedResult<GetPageListProductsResponse>>.Failure($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
        }
    }
}