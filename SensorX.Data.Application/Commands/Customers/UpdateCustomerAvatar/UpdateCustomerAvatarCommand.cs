using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerAvatar;

public sealed record UpdateCustomerAvatarCommand(
    string Avatar
) : IRequest<Result>;