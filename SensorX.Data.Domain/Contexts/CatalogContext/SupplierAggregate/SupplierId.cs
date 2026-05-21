using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;

public record SupplierId(Guid Value) : EntityId<SupplierId>(Value);
