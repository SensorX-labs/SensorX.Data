using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.StrongIDs;

public record AccountId(Guid Value) : EntityId<AccountId>(Value);