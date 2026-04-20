using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.QueryExtensions;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsHandler(
    IQueryBuilder<Product> _productBuilder,
    IQueryBuilder<Category> _categoryBuilder,
    IQueryBuilder<InternalPrice> _internalPriceBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListProductsQuery, Result<ProductCursorPagedResult>>
{
    public async Task<Result<ProductCursorPagedResult>> Handle(
        GetPageListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery =
                from product in _productBuilder.QueryAsNoTracking
                    .ApplySearch(request.SearchTerm)
                from category in _categoryBuilder.QueryAsNoTracking
                    .Where(x => x.Id == product.CategoryId).DefaultIfEmpty()
                from internalPrice in _internalPriceBuilder.QueryAsNoTracking
                    .Where(x => x.ProductId == product.Id).DefaultIfEmpty()
                select new GetPageListProductsQueryModel(product, category, internalPrice);

            var pagedQuery = sourceQuery.ApplyCursorPagination(
                request,
                x => x.Product.CreatedAt,
                x => x.Product.Id.Value
            )
            .OrderByDescending(x => x.Product.CreatedAt)
            .ThenByDescending(x => x.Product.Id.Value);

            var dtoQuery = pagedQuery.Select(x => new GetPageListProductsResponse(
                x.Product.Id.Value,
                x.Product.Code.Value,
                x.Product.Name,
                x.Product.Manufacture,
                x.Category != null ? x.Category.Name : "",
                x.InternalPrice != null ? x.InternalPrice.SuggestedPrice.Amount : 0,
                x.Product.Status,
                x.Product.CreatedAt,
                x.Product.Images.Select(i => i.ImageUrl).ToList()
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery
                .Take(request.PageSize + 1), cancellationToken);

            var hasNext = items.Count > request.PageSize;
            if (hasNext) items.RemoveAt(request.PageSize); // remove phần tử cuối cùng nếu có next page (kỹ thuật key-set pagination)

            var result = new ProductCursorPagedResult
            {
                Items = items,
                HasNext = hasNext,
                HasPrevious = request.IsPrevious,
                FirstCreatedAt = items.FirstOrDefault()?.CreatedAt,
                FirstId = items.FirstOrDefault()?.Id,
                LastCreatedAt = items.LastOrDefault()?.CreatedAt,
                LastId = items.LastOrDefault()?.Id
            };

            return Result<ProductCursorPagedResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<ProductCursorPagedResult>.Failure($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
        }
    }
}