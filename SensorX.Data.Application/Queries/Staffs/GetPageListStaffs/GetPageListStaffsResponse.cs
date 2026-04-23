using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public sealed record GetPageListStaffsResponse(
    Guid Id,
    string Code,
    string Name,
    string Email,
    string Phone,
    string CitizenId,
    string Department,
    DateTimeOffset CreatedAt
);

public sealed class StaffOffsetPagedResult : OffsetPagedResult<GetPageListStaffsResponse> { }