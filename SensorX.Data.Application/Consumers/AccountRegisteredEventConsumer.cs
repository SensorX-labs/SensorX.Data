using MassTransit;
using Microsoft.Extensions.Logging;
using SensorX.Gateway.Domain.Events;

namespace SensorX.Data.Application.Consumers;

public class AccountRegisteredEventConsumer : IConsumer<AccountRegisteredEvent>
{
    private readonly ILogger<AccountRegisteredEventConsumer> _logger;

    public AccountRegisteredEventConsumer(ILogger<AccountRegisteredEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<AccountRegisteredEvent> context)
    {
        var message = context.Message;
        
        if (message.AccountType == "customer")
        {
            _logger.LogInformation("Creating Customer profile for AccountId: {AccountId}, Email: {Email}", message.AccountId, message.Email);
        }
        else if (message.AccountType == "staff")
        {
            _logger.LogInformation("Creating Staff profile for AccountId: {AccountId}, Email: {Email}", message.AccountId, message.Email);
        }
        _logger.LogInformation("Successfully processed AccountRegisteredEvent [AccountId: {AccountId}, AccountType: {AccountType}]", 
            message.AccountId, message.AccountType);

        return Task.CompletedTask;
    }
}
