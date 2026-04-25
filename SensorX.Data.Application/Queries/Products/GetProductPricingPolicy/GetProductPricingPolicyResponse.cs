using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

public sealed record GetProductPricingPolicyResponse(
    // Domain properties - Product
    Guid ProductId,
    string ProductCode,
    string ProductName,
    string Manufacture,
    string Unit,
    ProductStatus ProductStatus,           // 0=Inactive, 1=Active

    // InternalPrice data
    decimal SuggestedPrice,
    decimal FloorPrice,
    List<ProductPriceTier> PriceTiers,

    // Timestamps
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);

public sealed record ProductPriceTier(
    int Quantity,
    decimal Price
);
