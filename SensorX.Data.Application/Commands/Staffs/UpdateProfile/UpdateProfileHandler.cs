using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate.Specs;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Staffs.UpdateProfile;

public class UpdateProfileHandler(
    IRepository<Staff> _staffRepository,
    ICurrentUser _currentUser,
    IPublishEndpoint _publishEndpoint
) : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var specification = new AccountIdSpec(new AccountId(_currentUser.UserId ?? Guid.Empty));
        var staff = await _staffRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (staff == null)
        {
            return Result.Failure("Không tìm thấy thông tin nhân viên cho tài khoản này.");
        }

        staff.UpdateProfile(
            request.Name,
            string.IsNullOrWhiteSpace(request.Phone) ? null : Phone.From(request.Phone),
            Email.From(request.Email),
            string.IsNullOrWhiteSpace(request.CitizenId) ? null : CitizenId.From(request.CitizenId),
            request.Biography,
            staff.JoinDate,
            staff.Department,
            staff.WarehouseId
        );

        await _publishEndpoint.Publish(new UpdateStaffEvent(
            staff.Id.Value,
            staff.Name,
            staff.Phone?.Value,
            staff.Email.Value,
            staff.CitizenId?.Value,
            staff.Biography,
            staff.JoinDate,
            staff.Department,
            staff.Status
        ), cancellationToken);

        await _staffRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Cập nhật hồ sơ thành công!");
    }
}
