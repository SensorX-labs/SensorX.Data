using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate.Specs;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerAvatar;

public sealed class UpdateCustomerAvatarHandler(
    IRepository<Customer> _customerRepository,
    ICloudinaryService _cloudinaryService,
    ICurrentUser _currentUser
) : IRequestHandler<UpdateCustomerAvatarCommand, Result>
{
    public async Task<Result> Handle(UpdateCustomerAvatarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var specification = new AccountIdSpec(new AccountId(_currentUser.UserId ?? Guid.Empty));
            var customer = await _customerRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (customer == null)
            {
                await _cloudinaryService.DeleteImageAsync(request.Avatar, cancellationToken);
                return Result.Failure("Không tìm thấy khách hàng.");
            }

            customer.UpdateAvatar(request.Avatar);

            await _customerRepository.SaveChangesAsync(cancellationToken);

            return Result.Success("Cập nhật ảnh đại diện thành công");
        }
        catch (Exception ex)
        {
            await _cloudinaryService.DeleteImageAsync(request.Avatar, cancellationToken);
            return Result.Failure($"Lỗi khi cập nhật ảnh đại diện: {ex.Message}");
        }
    }
}