namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceById;

public sealed record GetInternalPriceByIdResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductCode,
    string Unit,
    decimal SuggestedPriceAmount,
    string SuggestedPriceCurrency,
    decimal FloorPriceAmount,
    string FloorPriceCurrency,
    List<PriceTierDetailResponse> PriceTiers,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt
);

public sealed record PriceTierDetailResponse(
    int Quantity,
    decimal Price,
    string Currency
);
