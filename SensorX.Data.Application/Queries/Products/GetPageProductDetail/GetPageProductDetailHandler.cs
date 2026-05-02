using System.Net;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageProductDetail;

public sealed class GetPageProductDetailHandler(
    IQueryBuilder<Product> _productQueryBuilder,
    IQueryBuilder<Category> _categoryQueryBuilder,
    IQueryBuilder<InternalPrice> _internalPriceQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageProductDetailQuery, Result<GetPageProductDetailResponse>>
{
    public async Task<Result<GetPageProductDetailResponse>> Handle(GetPageProductDetailQuery request, CancellationToken cancellationToken)
    {
        var productDetailQuery = from p in _productQueryBuilder.QueryAsNoTracking.Where(x => x.Id == request.Id)
                                 join c in _categoryQueryBuilder.QueryAsNoTracking on p.CategoryId equals c.Id into categoryList
                                 from cat in categoryList.DefaultIfEmpty()
                                 select new { product = p, category = cat };

        var productResult = await _queryExecutor.FirstOrDefaultAsync(productDetailQuery, cancellationToken);
        if (productResult == null)
            return Result<GetPageProductDetailResponse>.Failure("Sản phẩm không tồn tại!");

        var product = productResult.product;
        var category = productResult.category;

        var internalPriceQuery = _internalPriceQueryBuilder.QueryAsNoTracking
                        .Where(x => (Guid)x.ProductId == request.Id)
                        .IsActive()
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenBy(x => x.ExpiresAt)
                        .ThenByDescending(x => x.Id);

        var dtoInternalPriceQuery = internalPriceQuery.Select(x => new InternalPriceDto(
            x.Id.Value,
            x.ProductId.Value,
            x.SuggestedPrice.Amount,
            x.SuggestedPrice.Currency,
            x.FloorPrice.Amount,
            x.FloorPrice.Currency,
            x.PriceTiers.Select(tier => new PriceTierDto(
                tier.Quantity.Value,
                tier.Price.Amount,
                tier.Price.Currency
            )).ToList(),
            x.CreatedAt
        ));

        var suggestedPrice = await _queryExecutor.FirstOrDefaultAsync(dtoInternalPriceQuery, cancellationToken);

        var response = new GetPageProductDetailResponse(
            product.Id,
            product.Code,
            product.Name,
            product.Manufacture,
            category?.Id ?? Guid.Empty,
            category?.Name ?? "",
            product.Unit,
            product.Showcase,
            product.Attributes.Select(x => new ProductAttributeResponse(x.AttributeName, x.AttributeValue)).ToList(),
            product.Status,
            product.CreatedAt,
            product.UpdatedAt,
            product.Images.Select(x => x.ImageUrl).ToList(),
            suggestedPrice
        );

        return Result<GetPageProductDetailResponse>.Success(response);
    }
}