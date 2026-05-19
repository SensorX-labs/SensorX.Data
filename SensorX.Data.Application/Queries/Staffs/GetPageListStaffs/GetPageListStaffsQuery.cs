using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public sealed record GetPageListStaffsQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListStaffsResponse>>>
{
    public string? SearchTerm { get; init; }
    public StaffStatus? Status { get; init; }
}