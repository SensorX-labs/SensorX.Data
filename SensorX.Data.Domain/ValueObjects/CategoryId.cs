using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.ValueObjects;

public record CategoryId(Guid Value) : EntityId<CategoryId>(Value);