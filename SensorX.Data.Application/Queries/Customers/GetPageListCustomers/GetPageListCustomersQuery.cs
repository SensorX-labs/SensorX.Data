using MediatR;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public record GetPageListCustomersQuery : CursorPagedQuery, IRequest<Result<CustomerCursorPagedResult>>
{
    public string? SearchTerm { get; init; }
}