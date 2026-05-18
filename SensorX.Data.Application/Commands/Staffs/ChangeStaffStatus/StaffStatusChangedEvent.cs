using System;
using MassTransit;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Commands.Staffs.ChangeStaffStatus;

[MessageUrn("staff-status-changed")]
[EntityName("staff-status-changed")]
public sealed record StaffStatusChangedEvent(
    Guid Id,
    Guid AccountId,
    StaffStatus Status
);
