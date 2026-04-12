using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public record PriceTier
{
    public Quantity Quantity { get; init; }
    public Money Price { get; init; }

    // Constructor cho EF Core
    private PriceTier() { }

    // Constructor cho nghiệp vụ
    public PriceTier(Quantity quantity, Money price)
    {
        Quantity = quantity;
        Price = price;
    }
}