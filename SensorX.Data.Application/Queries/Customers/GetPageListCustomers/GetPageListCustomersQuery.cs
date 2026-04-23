using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public record GetPageListCustomersQuery : OffsetPagedQuery, IRequest<Result<CustomerOffsetPagedResult>>
{
    public string? SearchTerm { get; init; }
}