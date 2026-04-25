using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;

public sealed record GetCustomerBuyingHistoryQuery(Guid CustomerId) : IRequest<Result<GetCustomerBuyingHistoryResponse>>;

