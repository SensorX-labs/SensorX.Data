using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.CreateCustomer;

public sealed record CreateCustomerCommand(
    Guid AccountId,
    string Name,
    string TaxCode,
    string Phone,
    string Email,
    string Address,
    Guid? WardId = null,
    string? ShippingAddress = null,
    string? ReceiverName = null,
    string? ReceiverPhone = null
) : IRequest<Result<Guid>>;
