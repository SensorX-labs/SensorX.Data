using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public record ProvinceId(Guid Value) : EntityId<ProvinceId>(Value);