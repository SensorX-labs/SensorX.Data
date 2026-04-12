using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public record InternalPriceId(Guid Value) : EntityId<InternalPriceId>(Value);