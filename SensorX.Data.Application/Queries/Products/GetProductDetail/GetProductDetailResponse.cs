using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetProductDetail;

public sealed record GetProductDetailResponse(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    string CategoryName,
    string Unit,
    string? Showcase,
    List<ProductAttributeResponse> Attributes,
    ProductStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    List<string> Images
);

public sealed record ProductAttributeResponse(
    string Name,
    string Value
);