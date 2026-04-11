using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.ValueObjects;

public record TransferOrderId(Guid Value) : EntityId<TransferOrderId>(Value);