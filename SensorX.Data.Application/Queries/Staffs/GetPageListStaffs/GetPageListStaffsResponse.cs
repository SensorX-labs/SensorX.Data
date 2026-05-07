using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public sealed record GetPageListStaffsResponse(
    Guid Id,
    string Code,
    string Name,
    string Email,
    string Phone,
    string CitizenId,
    Department Department,
    DateTimeOffset CreatedAt
);

