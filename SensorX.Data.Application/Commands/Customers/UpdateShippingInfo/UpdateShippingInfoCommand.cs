using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.UpdateShippingInfo;

public sealed record UpdateShippingInfoCommand(
    Guid Id,
    Guid? ProvinceId,
    Guid? WardId,
    string? ShippingAddress,
    string? ReceiverName,
    string? ReceiverPhone
) : IRequest<Result<Guid>>;