namespace SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;

public record GetDetailStaffByIdResponse(
    Guid StaffId,
    string StaffCode,
    string StaffName,
    string Email,
    string Phone,
    string Department,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
