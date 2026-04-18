namespace SensorX.Data.Application.Common.Dtos.Responses;

public class PriceTierResponse
{
    public int Quantity { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = string.Empty;
}