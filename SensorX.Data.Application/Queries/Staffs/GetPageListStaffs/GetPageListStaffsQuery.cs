using MediatR;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public record GetPageListStaffsQuery : CursorPagedQuery, IRequest<Result<StaffCursorPagedResult>>
{
    public string? SearchTerm { get; init; }
    public string? Code { get; init; }
}