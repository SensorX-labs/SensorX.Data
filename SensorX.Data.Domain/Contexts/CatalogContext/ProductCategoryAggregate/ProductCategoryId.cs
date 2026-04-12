using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;

public record ProductCategoryId(Guid Value) : EntityId<ProductCategoryId>(Value);