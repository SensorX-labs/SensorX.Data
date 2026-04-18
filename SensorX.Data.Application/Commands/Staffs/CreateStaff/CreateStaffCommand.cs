using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.CreateStaff;

public class CreateStaffCommand : IRequest<Result<Guid>>
{
    public Guid AccountId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CitizenId { get; set; }
    public string Biography { get; set; }
    public DateTime JoinDate { get; set; }
    public string Department { get; set; }
}
