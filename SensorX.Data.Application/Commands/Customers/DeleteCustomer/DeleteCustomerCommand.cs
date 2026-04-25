using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.DeleteCustomer;

public record DeleteCustomerCommand(Guid Id) : IRequest<Result<bool>>;
