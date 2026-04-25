using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.CreateStaff;

public sealed record CreateStaffCommand(
    Guid AccountId,
    string Code,
    string Name,
    string Phone,
    string Email,
    string CitizenId,
    string Biography,
    DateTime JoinDate,
    string Department
) : IRequest<Result<Guid>>;
