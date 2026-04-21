using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.CreateStaff;

public class CreateStaffCommand : IRequest<Result<Guid>>
{
    public Guid AccountId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string CitizenId { get; set; }
    public required string Biography { get; set; }
    public DateTime JoinDate { get; set; }
    public required string Department { get; set; }
}
