using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;

public record GetCustomerBuyingHistoryQuery(Guid CustomerId) : IRequest<Result<GetCustomerBuyingHistoryResponse>>;

