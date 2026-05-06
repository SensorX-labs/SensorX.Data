using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomer;

public class UpdateCustomerHandler(
    IRepository<Customer> _customerRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<UpdateCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerId = new CustomerId(request.Id);

        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
        {
            return Result<Guid>.Failure("Không tìm thấy khách hàng với ID tương ứng.");
        }

        customer.UpdateProfile(
            request.Name,
            Phone.From(request.Phone),
            Email.From(request.Email),
            request.TaxCode,
            request.Address
        );

        if (request.WardId.HasValue && !string.IsNullOrWhiteSpace(request.ShippingAddress) &&
            !string.IsNullOrWhiteSpace(request.ReceiverName) && !string.IsNullOrWhiteSpace(request.ReceiverPhone))
        {
            var shippingInfo = ShippingInfo.Create(
                new SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate.WardId(request.WardId.Value),
                request.ShippingAddress,
                request.ReceiverName,
                Phone.From(request.ReceiverPhone)
            );
            customer.UpdateShippingInfo(shippingInfo);
        }
        else if (!request.WardId.HasValue && string.IsNullOrWhiteSpace(request.ShippingAddress) &&
                 string.IsNullOrWhiteSpace(request.ReceiverName) && string.IsNullOrWhiteSpace(request.ReceiverPhone))
        {
            customer.UpdateShippingInfo(null);
        }

        await _customerRepository.Update(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(customer.Id.Value);
    }
}
