using MassTransit;

namespace SensorX.Data.Application.Events;

[MessageUrn("product-deleted")]
public record ProductDeletedEvent
{
    public Guid ProductId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}
