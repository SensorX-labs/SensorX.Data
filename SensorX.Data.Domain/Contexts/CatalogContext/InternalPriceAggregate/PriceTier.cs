using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public record PriceTier
{
    //tắt cảnh báo không dùng constructor
#pragma warning disable CS8618
    protected PriceTier() { } // EF Core
#pragma warning restore CS8618

    public PriceTier(Quantity quantity, Money price)
    {
        Quantity = quantity;
        Price = price;
    }
    public Quantity Quantity { get; init; }
    public Money Price { get; init; }
}