namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;

public sealed record GetInternalPricesByProductIdResponse(
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
    decimal PriceAmount,
    string PriceCurrency
);