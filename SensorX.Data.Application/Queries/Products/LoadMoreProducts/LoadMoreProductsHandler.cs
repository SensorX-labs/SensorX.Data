using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.LoadMore;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed class LoadMoreProductsHandler(
    IQueryBuilder<Product> productBuilder,
    IQueryBuilder<Category> categoryBuilder,
    IQueryBuilder<Supplier> supplierBuilder,
    IQueryBuilder<UnitOfQuantity> unitOfQuantityBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<LoadMoreProductsQuery, Result<LoadMoreResult<LoadMoreProductsResponse>>>
{
    public async Task<Result<LoadMoreResult<LoadMoreProductsResponse>>> Handle(LoadMoreProductsQuery request, CancellationToken cancellationToken)
    {
        var productQuery = productBuilder.QueryAsNoTracking;

        if (request.CategoryId.HasValue)
        {
            var categoryId = new CategoryId(request.CategoryId.Value);
            productQuery = productQuery.Where(x => x.CategoryId == categoryId);
        }

        var pagedProductBaseQuery = request.SortByName
            ? productQuery.ApplyLoadMoreWithOrder(request.LastValue, x => x.Name, request.LastId, x => (Guid)x.Id, request.IsDescending)
            : productQuery.ApplyLoadMoreWithOrder(request.LastValue.ToCursor<DateTimeOffset>(), x => x.CreatedAt, request.LastId, x => (Guid)x.Id, request.IsDescending);

        var sourceQuery = from product in pagedProductBaseQuery
                          join category in categoryBuilder.QueryAsNoTracking
                              on product.CategoryId equals category.Id into cs
                          from c in cs.DefaultIfEmpty()
                          join supplier in supplierBuilder.QueryAsNoTracking
                              on product.SupplierId equals supplier.Id into ss
                          from s in ss.DefaultIfEmpty()
                          join unit in unitOfQuantityBuilder.QueryAsNoTracking
                              on product.UnitOfQuantityId equals unit.Id into us
                          from u in us.DefaultIfEmpty()
                          select new
                          {
                              product,
                              categoryName = c != null ? c.Name : "",
                              supplierName = s != null ? s.Name : "",
                              unitOfQuantityName = u != null ? u.Name : ""
                          };

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            sourceQuery = sourceQuery.Where(x =>
                x.product.Name.ToLower().Contains(term) ||
                ((string)x.product.Code).ToLower().Contains(term) ||
                x.supplierName.ToLower().Contains(term));
        }

        var pageSize = request.PageSize ?? 10;
        var items = await queryExecutor.ToListAsync(sourceQuery.Take(pageSize + 1), cancellationToken);

        var hasNext = items.Count > pageSize;
        if (hasNext) items.RemoveAt(items.Count - 1);

        var responseItems = items.Select(x => new LoadMoreProductsResponse(
            (Guid)x.product.Id,
            (string)x.product.Code,
            x.product.Name,
            x.supplierName,
            x.unitOfQuantityName,
            x.product.CategoryId != null ? (Guid)x.product.CategoryId : null,
            x.categoryName,
            x.product.CreatedAt,
            x.product.Images.Select(i => i.ImageUrl).ToList()
        )).ToList();

        var lastItem = responseItems.LastOrDefault();

        return Result<LoadMoreResult<LoadMoreProductsResponse>>.Success(new LoadMoreResult<LoadMoreProductsResponse>
        {
            Items = responseItems,
            LastId = lastItem?.Id,
            LastValue = request.SortByName ? lastItem?.Name : lastItem?.CreatedAt.ToString("O"),
            HasNext = hasNext
        });
    }
}
