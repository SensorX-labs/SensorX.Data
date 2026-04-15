using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;

public record CategoryId(Guid Value) : EntityId<CategoryId>(Value);