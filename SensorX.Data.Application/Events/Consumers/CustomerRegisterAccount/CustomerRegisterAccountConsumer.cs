using MassTransit;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Events.Consumers.CustomerRegisterAccount;

public class CustomerRegisterAccountConsumer(
    IRepository<Customer> _customerRepository,
    ILogger<CustomerRegisterAccountConsumer> _logger
) : IConsumer<CustomerRegisterAccountEvent>
{
    public async Task Consume(ConsumeContext<CustomerRegisterAccountEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Consuming message: {Message}", message);

        var customer = new Customer(
            CustomerId.New(),
            new AccountId(message.AccountId),
            Code.Create("CUS"),
            message.Name,
            message.Phone == null ? null : Phone.Create(message.Phone),
            Email.Create(message.Email),
            message.TaxCode,
            message.Address
        );

        await _customerRepository.AddAsync(customer);
        _logger.LogInformation("Customer created: {Customer}", customer);
    }
}