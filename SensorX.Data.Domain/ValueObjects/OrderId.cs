using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.ValueObjects;

public record OrderId(Guid Value) : EntityId<OrderId>(Value);