using MassTransit;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaff;

[MessageUrn("staff-updated")]
[EntityName("staff-updated")]
public sealed record UpdateStaffEvent(
    Guid Id,
    string Name,
    string? Phone,
    string Email,
    string? CitizenId,
    string? Biography,
    DateTimeOffset JoinDate,
    Department Department
);