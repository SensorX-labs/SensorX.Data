using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
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
            var productQuery = _productBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

            // Phân trang và sắp xếp trên Product trước để tránh lỗi dịch LINQ phức tạp
            var pagedProductBaseQuery = productQuery.ApplyCursorPagination(
                request,
                x => x.CreatedAt,
                x => x.Id
            )
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id);

            // Sau đó mới Join để lấy thông tin bổ sung (Left Join)
            var sourceQuery = from product in pagedProductBaseQuery
                              join category in _categoryBuilder.QueryAsNoTracking
                                  on product.CategoryId equals category.Id into cs
                              from c in cs.DefaultIfEmpty()
                              join internalPrice in _internalPriceBuilder.QueryAsNoTracking
                                  on product.Id equals internalPrice.ProductId into ips
                              from i in ips.DefaultIfEmpty()
                              select new { product, category = c, internalPrice = i };

            var dtoQuery = sourceQuery.Select(x => new GetPageListProductsResponse(
                x.product.Id.Value,
                x.product.Code.Value,
                x.product.Name,
                x.product.Manufacture,
                x.category != null ? x.category.Name : "",
                x.internalPrice != null ? x.internalPrice.SuggestedPrice.Amount : 0,
                x.product.Status,
                x.product.CreatedAt,
                x.product.Images.Select(i => i.ImageUrl).ToList()
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery
                .Take(request.PageSize + 1), cancellationToken);

            var hasNext = items.Count > request.PageSize;
            if (hasNext) items.RemoveAt(request.PageSize);

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