using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Commands.Staffs.ChangeStaffStatus;

public class ChangeStaffStatusHandler(
    IRepository<Staff> _staffRepository,
    IPublishEndpoint _publishEndpoint
) : IRequestHandler<ChangeStaffStatusCommand, Result>
{
    public async Task<Result> Handle(ChangeStaffStatusCommand request, CancellationToken cancellationToken)
    {
        var staffId = new StaffId(request.Id);
        var staff = await _staffRepository.GetByIdAsync(staffId, cancellationToken);
        if (staff == null)
        {
            return Result.Failure("Không tìm thấy thông tin nhân viên.");
        }

        staff.ChangeStatus(request.Status);

        await _publishEndpoint.Publish(new StaffStatusChangedEvent(
            staff.Id.Value,
            staff.AccountId.Value,
            staff.Status
        ), cancellationToken);

        await _staffRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Cập nhật trạng thái nhân viên thành công.");
    }
}
