using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Customers.GetCustomerById;

namespace SensorX.Data.Application.Queries.Customers.GetDetailCustomerByAccountId;

public sealed record GetDetailCustomerByAccountIdQuery(Guid AccountId) : IRequest<Result<GetCustomerByIdResponse>>;
