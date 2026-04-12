using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public record WardId(Guid Value) : EntityId<WardId>(Value);