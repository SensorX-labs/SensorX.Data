using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerAvatar;

public sealed record UpdateCustomerAvatarCommand(
    [property: JsonIgnore] Guid Id,
    string Avatar
) : IRequest<Result>;