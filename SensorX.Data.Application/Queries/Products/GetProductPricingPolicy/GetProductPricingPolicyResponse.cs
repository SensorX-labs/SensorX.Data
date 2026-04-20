namespace SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

public record GetProductPricingPolicyResponse(
    // Domain properties - Product
    Guid ProductId,
    string ProductCode,
    string ProductName,
    string Manufacture,
    string Unit,
    int ProductStatus,           // 0=Inactive, 1=Active
    
    // InternalPrice data
    decimal FloorPrice,
    decimal SuggestedPrice,
    List<ProductPriceTier> PriceTiers,
    
    // Timestamps
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ProductPriceTier(
    int Quantity,
    decimal Price
);
