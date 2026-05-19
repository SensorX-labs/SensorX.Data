namespace SensorX.Data.Application.Queries.Staffs.GetStaffListStats;

public sealed record GetStaffListStatsResponse(
    int TotalCount,
    int ActiveCount,
    int OnLeaveCount,
    int ResignedCount
);
