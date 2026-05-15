using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaffAvatar;

public sealed record UpdateStaffAvatarCommand(
    [property: JsonIgnore] Guid Id,
    string Avatar
) : IRequest<Result>;