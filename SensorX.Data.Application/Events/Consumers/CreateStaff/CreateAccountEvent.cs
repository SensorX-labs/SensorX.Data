using MassTransit;
using SensorX.Data.Application.Common.Interfaces;

namespace SensorX.Data.Application.Events.Consumers.CreateStaff;

[MessageUrn("account-created")]
[EntityName("account-created")]
public record CreateAccountEvent
{
    public Guid AccountId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public Role Role { get; init; }
    public Guid? WarehouseId { get; init; }
    public DateTimeOffset RegisteredAt { get; init; }
}
