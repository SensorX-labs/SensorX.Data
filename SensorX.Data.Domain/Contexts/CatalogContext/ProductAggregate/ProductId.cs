using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public record ProductId(Guid Value) : EntityId<ProductId>(Value);