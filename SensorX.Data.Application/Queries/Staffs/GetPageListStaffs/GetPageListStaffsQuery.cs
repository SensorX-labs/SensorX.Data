using MediatR;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public record GetPageListStaffsQuery(
    string? SearchTerm,
    string? Code
) : CursorPagedQuery, IRequest<Result<StaffCursorPagedResult>>;