using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Customers.CreateCustomer;

public class CreateCustomerHandler(
    IRepository<Customer> _customerRepository
) : IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var id = new CustomerId(Guid.NewGuid());
        var code = Code.Create("CUS");
        var customer = new Customer(
            id,
            new AccountId(request.AccountId),
            code,
            request.Name,
            Phone.From(request.Phone),
            Email.From(request.Email),
            request.TaxCode,
            request.Address
        );
        customer.UpdateShippingInfo(ShippingInfo.Create(
            new WardId(request.WardId),
            request.ShippingAddress,
            request.ReceiverName,
            Phone.From(request.ReceiverPhone)
        ));
        await _customerRepository.AddAsync(customer, cancellationToken);
        return Result<Guid>.Success(customer.Id.Value);
    }
}