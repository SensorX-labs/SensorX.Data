using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;

public record CustomerId(Guid Value) : EntityId<CustomerId>(Value);