using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public record PriceTier(Quantity Quantity, Money Price);