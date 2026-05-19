using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaffAvatar;

public sealed record UpdateStaffAvatarCommand(
    string Avatar
) : IRequest<Result>;