using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerInfo;

public sealed record UpdateCustomerInfoCommand(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    string TaxCode,
    string Address
) : IRequest<Result<Guid>>;

