namespace SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

public sealed record GetStaffMetricsResponse(
    Guid StaffId,
    string StaffCode,
    string StaffName,
    string Email,
    string Phone,
    string Department,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
