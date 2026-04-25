using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Staffs.DeleteStaff;

public record DeleteStaffCommand(Guid Id) : IRequest<Result<bool>>;
