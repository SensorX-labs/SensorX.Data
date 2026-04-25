using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public sealed record GetPageListStaffsQuery : OffsetPagedQuery, IRequest<Result<StaffOffsetPagedResult>>
{
    public string? SearchTerm { get; init; }
}