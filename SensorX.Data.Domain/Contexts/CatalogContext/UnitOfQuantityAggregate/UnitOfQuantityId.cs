using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

public record UnitOfQuantityId(Guid Value) : EntityId<UnitOfQuantityId>(Value);
