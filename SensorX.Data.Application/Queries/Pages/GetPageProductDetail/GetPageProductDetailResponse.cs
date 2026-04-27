using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Pages.GetPageProductDetail;

public sealed record GetPageProductDetailResponse
(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    Guid? CategoryId,
    string? CategoryName,
    string Unit,
    string? Showcase,
    List<ProductAttributeResponse> Attributes,
    ProductStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    List<string> Images,
    InternalPriceDto? InternalPricesSuggestion
);

public sealed record InternalPriceDto(
    Guid Id,
    Guid ProductId,
    decimal SuggestedPriceAmount,
    string SuggestedPriceCurrency,
    decimal FloorPriceAmount,
    string FloorPriceCurrency,
    List<PriceTierDto> PriceTiers,
    DateTimeOffset CreatedAt
);

public sealed record PriceTierDto(
    int Quantity,
    decimal Amount,
    string Currency
);

public sealed record ProductAttributeResponse(
    string Name,
    string Value
);