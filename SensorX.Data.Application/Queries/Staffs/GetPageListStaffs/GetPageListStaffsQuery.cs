using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public sealed record GetPageListStaffsQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListStaffsResponse>>>
{
    public string? SearchTerm { get; init; }
    public StaffStatus? Status { get; init; }
    public string? Code { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? CitizenId { get; init; }
    public string? Department { get; init; }
    public DateTimeOffset? JoinFrom { get; init; }
    public DateTimeOffset? JoinTo { get; init; }
    public DateTimeOffset? CreatedFrom { get; init; }
    public DateTimeOffset? CreatedTo { get; init; }
}
