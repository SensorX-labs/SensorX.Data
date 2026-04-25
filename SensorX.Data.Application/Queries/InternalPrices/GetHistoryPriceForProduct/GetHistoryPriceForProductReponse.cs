using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.InternalPrices.GetHistoryPriceForProduct;

public sealed record GetHistoryPriceForProductResponse(
    Guid ProductId,
    string ProductCode,
    string ProductName,
    List<InternalPriceDto> InternalPrices
);

public sealed record InternalPriceDto(
    Guid Id,
    decimal SuggestedPrice,
    string SuggestedPriceCurrency,
    decimal FloorPrice,
    string FloorPriceCurrency,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt,
    List<PriceTierDto> PriceTiers
);

public sealed record PriceTierDto(
    int Quantity,
    decimal Price,
    string Currency
);