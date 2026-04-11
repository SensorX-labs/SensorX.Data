
using SensorX.Data.Domain.SeedWork;
namespace SensorX.Data.Domain.ValueObjects;

public record ProductId(Guid Value) : EntityId<ProductId>(Value);