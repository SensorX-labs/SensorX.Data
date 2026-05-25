using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public sealed record GetPageListCustomersQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListCustomersResponse>>>
{
    public string? SearchTerm { get; init; }
    public string? Code { get; init; }
    public string? Name { get; init; }
    public string? TaxCode { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
}
