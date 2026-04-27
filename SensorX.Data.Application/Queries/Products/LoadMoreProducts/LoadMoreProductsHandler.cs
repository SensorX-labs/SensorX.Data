using System.ComponentModel;
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
) : IRequestHandler<LoadMoreProductsQuery, Result<KeysetPagedResult<LoadMoreProductsResponse>>>
{
    public async Task<Result<KeysetPagedResult<LoadMoreProductsResponse>>> Handle(LoadMoreProductsQuery request, CancellationToken cancellationToken)
    {
        var productQuery = _productBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

        var pagedProductBaseQuery = productQuery.ApplyKeysetPaginationWithOrder(request.LastValue.ToCursor<DateTimeOffset>(), x => x.CreatedAt, request.LastId, x => (Guid)x.Id, request.IsDescending);

        var sourceQuery = from product in pagedProductBaseQuery
                          join category in _categoryBuilder.QueryAsNoTracking
                              on product.CategoryId equals category.Id into cs
                          from c in cs.DefaultIfEmpty()
                          select new { product, categoryName = c != null ? c.Name : "" };

        var pageSize = request.PageSize ?? 10;
        var items = await _queryExecutor.ToListAsync(sourceQuery.Take(pageSize + 1), cancellationToken);

        bool hasNext = items.Count > pageSize;
        if (hasNext) items.RemoveAt(items.Count - 1);

        var responseItems = items.Select(x => new LoadMoreProductsResponse(
            (Guid)x.product.Id,
            (string)x.product.Code,
            x.product.Name,
            x.product.Manufacture,
            x.product.CategoryId != null ? (Guid)x.product.CategoryId : null,
            x.categoryName,
            x.product.CreatedAt,
            x.product.Images.Select(i => i.ImageUrl).ToList()
        )).ToList();

        var lastItem = responseItems.LastOrDefault();

        var result = new KeysetPagedResult<LoadMoreProductsResponse>
        {
            Items = responseItems,
            LastId = lastItem?.Id,
            LastValue = lastItem?.CreatedAt.ToString("O"),
            HasNext = hasNext
        };

        return Result<KeysetPagedResult<LoadMoreProductsResponse>>.Success(result);
    }
}