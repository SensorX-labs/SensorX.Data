using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

public sealed record GetStaffMetricsResponse(
    Guid StaffId,
    string StaffCode,
    string StaffName,
    string Email,
    string Phone,
    Department Department,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
