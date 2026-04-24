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
    IUnitOfWork _unitOfWork
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

        Department? department = null;
        if (!string.IsNullOrWhiteSpace(request.Department))
        {
            if (Enum.TryParse<Department>(request.Department, true, out var parsedDept))
            {
                department = parsedDept;
            }
            else
            {
                return Result<Guid>.Failure("Phòng ban không hợp lệ.");
            }
        }

        staff.UpdateProfile(
            request.Name,
            string.IsNullOrWhiteSpace(request.Phone) ? null : Phone.From(request.Phone),
            Email.From(request.Email),
            string.IsNullOrWhiteSpace(request.CitizenId) ? null : CitizenId.From(request.CitizenId),
            request.Biography,
            request.JoinDate,
            department
        );

        await _staffRepository.Update(staff, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(staff.Id.Value);
    }
}
