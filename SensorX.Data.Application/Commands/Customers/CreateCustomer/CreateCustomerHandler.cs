using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Application.Commands.Customers.CreateCustomer;

public class CreateCustomerHandler(
    IRepository<Customer> _customerRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var id = new CustomerId(Guid.NewGuid());
        var customer = new Customer(
            id,
            new AccountId(request.AccountId),
            Code.From(request.Code),
            request.Name,
            Phone.From(request.Phone),
            Email.From(request.Email),
            request.TaxCode,
            request.Address,
            ShippingInfo.Create(
                new WardId(request.WardId),
                request.ShippingAddress,
                request.ReceiverName,
                Phone.From(request.ReceiverPhone)
            )
        );
        await _customerRepository.Add(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(customer.Id.Value);
    }
}