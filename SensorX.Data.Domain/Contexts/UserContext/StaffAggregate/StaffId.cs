using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

public record StaffId(Guid Value) : EntityId<StaffId>(Value);