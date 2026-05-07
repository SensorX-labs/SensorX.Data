namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceSuggest;

public sealed record InternalPriceResponse(
    Guid Id,
    Guid ProductId,
    decimal SuggestedPriceAmount,
    string SuggestedPriceCurrency,
    decimal FloorPriceAmount,
    string FloorPriceCurrency,
    List<PriceTierResponse> PriceTiers,
    DateTimeOffset CreatedAt
);

public sealed record PriceTierResponse(
    int Quantity,
    decimal Amount,
    string Currency
);