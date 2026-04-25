using MassTransit;
using Microsoft.Extensions.Logging;
using SensorX.Gateway.Domain.Events;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace SensorX.Data.Application.Consumers;

public class AccountRegisteredEventConsumer : IConsumer<AccountRegisteredEvent>
{
    private readonly ILogger<AccountRegisteredEventConsumer> _logger;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Staff> _staffRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountRegisteredEventConsumer(
        ILogger<AccountRegisteredEventConsumer> logger,
        IRepository<Customer> customerRepository,
        IRepository<Staff> staffRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _staffRepository = staffRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<AccountRegisteredEvent> context)
    {
        var message = context.Message;
        
        if (message.AccountType == "customer")
        {
            var customer = new Customer(
                new CustomerId(Guid.NewGuid()),
                new AccountId(message.AccountId),
                Code.Create("CUS"),
                message.FullName,
                null, // Phone chưa có
                Email.From(message.Email),
                null, // TaxCode chưa có
                null  // Address chưa có
            );
            await _customerRepository.Add(customer, context.CancellationToken);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Creating Customer profile for AccountId: {AccountId}, Email: {Email}", message.AccountId, message.Email);
        }
        else if (message.AccountType == "staff")
        {
            var staff = new Staff(
                new StaffId(Guid.NewGuid()),
                new AccountId(message.AccountId),
                Code.Create("STF"),
                message.FullName,
                null, // Phone chưa có
                Email.From(message.Email),
                null, // CitizenId chưa có
                null, // Biography chưa có
                DateTimeOffset.UtcNow,
                null // Department Enum mặc định
            );
            await _staffRepository.Add(staff, context.CancellationToken);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Creating Staff profile for AccountId: {AccountId}, Email: {Email}", message.AccountId, message.Email);
        }
        _logger.LogInformation("Successfully processed AccountRegisteredEvent [AccountId: {AccountId}, AccountType: {AccountType}]", 
            message.AccountId, message.AccountType);
    }
}
