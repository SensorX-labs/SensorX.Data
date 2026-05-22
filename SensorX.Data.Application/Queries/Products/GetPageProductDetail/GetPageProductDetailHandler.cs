using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageProductDetail;

public sealed class GetPageProductDetailHandler(
    IQueryBuilder<Product> productQueryBuilder,
    IQueryBuilder<Category> categoryQueryBuilder,
    IQueryBuilder<Supplier> supplierQueryBuilder,
    IQueryBuilder<UnitOfQuantity> unitOfQuantityQueryBuilder,
    IQueryBuilder<InternalPrice> internalPriceQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetPageProductDetailQuery, Result<GetPageProductDetailResponse>>
{
    public async Task<Result<GetPageProductDetailResponse>> Handle(GetPageProductDetailQuery request, CancellationToken cancellationToken)
    {
        var productDetailQuery = from p in productQueryBuilder.QueryAsNoTracking.Where(x => x.Id == request.Id)
                                 join c in categoryQueryBuilder.QueryAsNoTracking on p.CategoryId equals c.Id into categoryList
                                 from cat in categoryList.DefaultIfEmpty()
                                 join s in supplierQueryBuilder.QueryAsNoTracking on p.SupplierId equals s.Id into supplierJoin
                                 from supplierItem in supplierJoin.DefaultIfEmpty()
                                 join u in unitOfQuantityQueryBuilder.QueryAsNoTracking on p.UnitOfQuantityId equals u.Id into unitJoin
                                 from unitItem in unitJoin.DefaultIfEmpty()
                                 select new { product = p, category = cat, supplier = supplierItem, unit = unitItem };

        var productResult = await queryExecutor.FirstOrDefaultAsync(productDetailQuery, cancellationToken);
        if (productResult == null)
            return Result<GetPageProductDetailResponse>.Failure("Sản phẩm không tồn tại!");

        var product = productResult.product;
        var category = productResult.category;
        var supplier = productResult.supplier;
        var unit = productResult.unit;

        var internalPriceQuery = internalPriceQueryBuilder.QueryAsNoTracking
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

        var suggestedPrice = await queryExecutor.FirstOrDefaultAsync(dtoInternalPriceQuery, cancellationToken);

        var response = new GetPageProductDetailResponse(
            product.Id,
            product.Code,
            product.Name,
            product.SupplierId.Value,
            supplier?.Name ?? "",
            category?.Id ?? Guid.Empty,
            category?.Name ?? "",
            product.UnitOfQuantityId.Value,
            unit?.Name ?? "",
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
