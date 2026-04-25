using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.KeysetPagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed class LoadMoreProductsHandler(
    IQueryBuilder<Product> _productBuilder,
    IQueryBuilder<Category> _categoryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<LoadMoreProductsQuery, Result<LoadMoreProductsResult>>
{
    public async Task<Result<LoadMoreProductsResult>> Handle(LoadMoreProductsQuery request, CancellationToken cancellationToken)
    {
        var productQuery = _productBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

        // Get total count before pagination
        var totalCount = await _queryExecutor.CountAsync(productQuery, cancellationToken);

        // Apply ordering and pagination
        var pagedProductBaseQuery = productQuery
            .ApplyKeysetPagination(request, x => x.CreatedAt, x => x.Id)
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id);

        // Join to get additional info
        var sourceQuery = from product in pagedProductBaseQuery
                          join category in _categoryBuilder.QueryAsNoTracking
                              on product.CategoryId equals category.Id into cs
                          from c in cs.DefaultIfEmpty()
                          select new { product, category = c };

        var dtoQuery = sourceQuery.Select(x => new LoadMoreProductsResponse(
            x.product.Id.Value,
            x.product.Code.Value,
            x.product.Name,
            x.product.Manufacture,
            x.category != null ? x.category.Name : "",
            x.product.CreatedAt,
            x.product.Images.Select(i => i.ImageUrl).ToList()
        ));

        var items = await _queryExecutor.ToListAsync(dtoQuery.Take(request.PageSize + 1), cancellationToken);

        // Nếu số lượng lấy ra > PageSize, nghĩa là còn dữ liệu ở hướng đang query
        bool hasMore = items.Count > request.PageSize;
        if (hasMore) items.RemoveAt(items.Count - 1);

        var result = new LoadMoreProductsResult
        {
            Items = items,
            FirstCreatedAt = items.FirstOrDefault()?.CreatedAt,
            FirstId = items.FirstOrDefault()?.Id,
            LastCreatedAt = items.LastOrDefault()?.CreatedAt,
            LastId = items.LastOrDefault()?.Id,
            HasNext = request.IsPrevious || hasMore,
            HasPrevious = request.IsPrevious ? hasMore : (request.LastCreatedAt.HasValue || request.LastId.HasValue)
        };

        return Result<LoadMoreProductsResult>.Success(result);
    }
}