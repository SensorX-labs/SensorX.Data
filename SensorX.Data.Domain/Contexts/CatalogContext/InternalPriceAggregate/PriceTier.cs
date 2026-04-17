using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public record PriceTier
{
    public Quantity Quantity { get; init; }
    public Money Price { get; init; }

    protected PriceTier() { }
    public PriceTier(Quantity quantity, Money price)
    {
        Quantity = quantity;
        Price = price;
    }
}