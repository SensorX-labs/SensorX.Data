using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.UpdateProfile;

public class UpdateProfileCommand : IRequest<Result>
{
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public required string Email { get; set; }
    public string? CitizenId { get; set; }
    public string? Biography { get; set; }
}
