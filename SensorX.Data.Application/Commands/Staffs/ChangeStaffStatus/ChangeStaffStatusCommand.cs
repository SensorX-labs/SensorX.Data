using System;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Commands.Staffs.ChangeStaffStatus;

public class ChangeStaffStatusCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public StaffStatus Status { get; set; }
}
