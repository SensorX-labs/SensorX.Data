namespace SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

public record GetStaffMetricsResponse(
    Guid StaffId,
    string StaffCode,
    string StaffName,
    string Email,
    string Phone,
    string Department,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
