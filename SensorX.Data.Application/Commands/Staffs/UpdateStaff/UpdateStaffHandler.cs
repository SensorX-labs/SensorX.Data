using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaff;

public class UpdateStaffHandler(
    IRepository<Staff> _staffRepository,
    IPublishEndpoint _publishEndpoint
) : IRequestHandler<UpdateStaffCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var staffId = new StaffId(request.Id);

        var staff = await _staffRepository.GetByIdAsync(staffId, cancellationToken);
        if (staff == null)
        {
            return Result<Guid>.Failure("Không tìm thấy nhân viên với ID tương ứng.");
        }

        staff.UpdateProfile(
            request.Name,
            string.IsNullOrWhiteSpace(request.Phone) ? null : Phone.From(request.Phone),
            Email.From(request.Email),
            string.IsNullOrWhiteSpace(request.CitizenId) ? null : CitizenId.From(request.CitizenId),
            request.Biography,
            request.JoinDate,
            request.Department
        );

        await _publishEndpoint.Publish(new UpdateStaffEvent(
            staff.Id,
            staff.Name,
            staff.Phone,
            staff.Email,
            staff.CitizenId,
            staff.Biography,
            staff.JoinDate,
            staff.Department
        ), cancellationToken);

        await _staffRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(staff.Id.Value);
    }
}
