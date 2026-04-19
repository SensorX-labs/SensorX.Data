using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsHandler(
    IReadRepository<Product> _productRepository,
    IReadRepository<Category> _categoryRepository,
    IReadRepository<InternalPrice> _internalPriceRepository
) : IRequestHandler<GetPageListProductsQuery, Result<ProductCursorPagedResult>>
{
    public async Task<Result<ProductCursorPagedResult>> Handle(
        GetPageListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery =
                from product in _productRepository.QueryAsNoTracking
                from category in _categoryRepository.QueryAsNoTracking
                    .Where(x => x.Id == product.CategoryId).DefaultIfEmpty()
                from internalPrice in _internalPriceRepository.QueryAsNoTracking
                    .Where(x => x.ProductId == product.Id).DefaultIfEmpty()
                select new { product, category, internalPrice };

            var pagedQuery = sourceQuery.ApplyCursorPagination(
                request,
                x => x.product.CreatedAt,
                x => x.product.Id.Value
            )
            .OrderByDescending(x => x.product.CreatedAt)
            .ThenByDescending(x => x.product.Id.Value);

            var dtoQuery = pagedQuery.Select(x => new GetPageListProductsResponse(
                x.product.Id.Value,
                x.product.Code.Value,
                x.product.Name,
                x.product.Manufacture,
                x.category != null ? x.category.Name : null,
                x.internalPrice != null ? x.internalPrice.SuggestedPrice.Amount : 0,
                x.product.Status,
                x.product.CreatedAt,
                x.product.Images.Select(i => i.ImageUrl).ToList()
            ));

            var items = await dtoQuery
                .Take(request.PageSize + 1) // +1 để check có next page không
                .ToListAsync(cancellationToken);

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