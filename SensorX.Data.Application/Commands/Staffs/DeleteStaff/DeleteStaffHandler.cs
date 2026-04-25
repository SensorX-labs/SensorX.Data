using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Commands.Staffs.DeleteStaff;

public class DeleteStaffHandler(
    IRepository<Staff> _staffRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<DeleteStaffCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var staffId = new StaffId(request.Id);
        
        var staff = await _staffRepository.GetByIdAsync(staffId, cancellationToken);
        if (staff == null)
        {
            return Result<bool>.Failure("Không tìm thấy nhân viên với ID tương ứng.");
        }

        await _staffRepository.Delete(staff, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
