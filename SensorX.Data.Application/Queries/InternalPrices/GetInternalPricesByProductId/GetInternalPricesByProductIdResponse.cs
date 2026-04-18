namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;

public class GetInternalPricesByProductIdResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    
    public decimal SuggestedPriceAmount { get; set; }
    public string SuggestedPriceCurrency { get; set; } = string.Empty;
    
    public decimal FloorPriceAmount { get; set; }
    public string FloorPriceCurrency { get; set; } = string.Empty;
    
    public List<PriceTierResponse> PriceTiers { get; set; } = [];
    
    public DateTimeOffset CreatedAt { get; set; }
}

public class PriceTierResponse
{
    public int Quantity { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = string.Empty;
}
