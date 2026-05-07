using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.Enums;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Events.Consumers.CreateAccount;

public class CreateAccountConsumer(
    ILogger<CreateAccountConsumer> _logger,
    IRepository<Staff> _staffRepository,
    IUnitOfWork _unitOfWork
) : IConsumer<CreateAccountEvent>
{
    public async Task Consume(ConsumeContext<CreateAccountEvent> context)
    {
        var message = context.Message;
        var department = message.Role switch
        {
            Role.Manager => Department.Manager,
            Role.WarehouseStaff => Department.Warehouse,
            Role.SaleStaff => Department.Sale,
            _ => throw new InvalidOperationException($"Không tìm thấy department cho role {message.Role}")
        };
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
            department
        );
        await _staffRepository.Add(staff, context.CancellationToken);
        await _unitOfWork.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("Creating Staff profile for AccountId: {AccountId}, Email: {Email}", message.AccountId, message.Email);
    }
}
