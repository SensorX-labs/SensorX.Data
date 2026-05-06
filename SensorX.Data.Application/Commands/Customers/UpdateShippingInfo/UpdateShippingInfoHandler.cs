using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Customers.UpdateShippingInfo;

public class UpdateShippingInfoHandler(
    IRepository<Customer> _customerRepository
) : IRequestHandler<UpdateShippingInfoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateShippingInfoCommand request, CancellationToken cancellationToken)
    {
        var customerId = new CustomerId(request.Id);

        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);

        if (customer is null)
        {
            return Result<Guid>.Failure("Không tìm thấy khách hàng");
        }

        if (request.ProvinceId.HasValue && request.WardId.HasValue &&
            !string.IsNullOrWhiteSpace(request.ShippingAddress) &&
            !string.IsNullOrWhiteSpace(request.ReceiverName) &&
            !string.IsNullOrWhiteSpace(request.ReceiverPhone))
        {
            var shippingInfo = ShippingInfo.Create(
                new ProvinceId(request.ProvinceId.Value),
                new WardId(request.WardId.Value),
                request.ShippingAddress,
                request.ReceiverName,
                Phone.From(request.ReceiverPhone)
            );
            customer.UpdateShippingInfo(shippingInfo);
        }
        else
        {
            customer.UpdateShippingInfo(null);
        }

        await _customerRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(customer.Id.Value);
    }
}
