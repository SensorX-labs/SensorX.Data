using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;

public sealed record GetPageListInternalPriceResponse(
    Guid Id,
    Guid ProductId,
    string ProductCode,
    string ProductName,
    decimal SuggestedPrice,
    string SuggestedPriceCurrency,
    decimal FloorPrice,
    string FloorPriceCurrency,
    InternalPriceStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt,
    List<PriceTierDto> PriceTiers
);

public sealed record PriceTierDto(
    int Quantity,
    decimal Price,
    string Currency
);

public enum InternalPriceStatus
{
    Active,
    ExpiringSoon,
    Expired,
}

public sealed class InternalPriceOffsetPagedResult : OffsetPagedResult<GetPageListInternalPriceResponse>;