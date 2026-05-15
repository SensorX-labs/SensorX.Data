using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerInfo;

public class UpdateCustomerInfoHandler(
    IRepository<Customer> _customerRepository,
    IPublishEndpoint _publishEndpoint
) : IRequestHandler<UpdateCustomerInfoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateCustomerInfoCommand request, CancellationToken cancellationToken)
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

        await _publishEndpoint.Publish(new UpdateCustomerInfoEvent(
            customer.Id,
            customer.Name,
            customer.Phone,
            customer.Email,
            customer.TaxCode,
            customer.Address
        ), cancellationToken);

        await _customerRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(customer.Id.Value, "Cập nhật hồ sơ doanh nghiệp thành công");
    }
}
