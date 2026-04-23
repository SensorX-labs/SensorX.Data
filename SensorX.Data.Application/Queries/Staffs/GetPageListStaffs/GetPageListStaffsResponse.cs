using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public class GetPageListStaffsResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string CitizenId { get; set; } = null!;
    public string Department { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}

public class StaffOffsetPagedResult : OffsetPagedResult<GetPageListStaffsResponse> { }