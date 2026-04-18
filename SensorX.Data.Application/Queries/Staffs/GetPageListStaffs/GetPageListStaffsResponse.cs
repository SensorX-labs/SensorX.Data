namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public class GetPageListStaffsResponse
{
    public Guid Id { get; set; }
    public string AccountId { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CitizenId { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public DateTime JoinDate { get; set; }
    public string Department { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}
