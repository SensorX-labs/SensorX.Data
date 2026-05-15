using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Staffs.UpdateStaffAvatar;

public sealed class UpdateStaffAvatarHandler(
    IRepository<Staff> _staffRepository,
    ICloudinaryService _cloudinaryService
) : IRequestHandler<UpdateStaffAvatarCommand, Result>
{
    public async Task<Result> Handle(UpdateStaffAvatarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var staff = await _staffRepository.GetByIdAsync(new StaffId(request.Id), cancellationToken);
            if (staff == null)
            {
                await _cloudinaryService.DeleteImageAsync(request.Avatar, cancellationToken);
                return Result.Failure("Không tìm thấy nhân viên.");
            }

            staff.UpdateAvatar(request.Avatar);

            await _staffRepository.SaveChangesAsync(cancellationToken);

            return Result.Success("Cập nhật ảnh đại diện thành công");
        }
        catch (Exception ex)
        {
            await _cloudinaryService.DeleteImageAsync(request.Avatar, cancellationToken);
            return Result.Failure($"Lỗi khi cập nhật ảnh đại diện: {ex.Message}");
        }
    }
}