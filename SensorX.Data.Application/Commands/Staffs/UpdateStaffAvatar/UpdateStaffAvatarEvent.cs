using MassTransit;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaffAvatar;

[MessageUrn("staff-avatar-updated")]
[EntityName("staff-avatar-updated")]
public sealed record UpdateStaffAvatarEvent(
    Guid Id,
    string AvatarUrl
);