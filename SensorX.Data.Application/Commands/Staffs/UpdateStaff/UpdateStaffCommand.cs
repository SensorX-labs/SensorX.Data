using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaff;

public class UpdateStaffCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public required string Email { get; set; }
    public string? CitizenId { get; set; }
    public string? Biography { get; set; }
    public DateTimeOffset JoinDate { get; set; }
    public string? Department { get; set; }
}
