using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public sealed record GetPageListCustomersQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListCustomersResponse>>>
{
    public string? SearchTerm { get; init; }
}