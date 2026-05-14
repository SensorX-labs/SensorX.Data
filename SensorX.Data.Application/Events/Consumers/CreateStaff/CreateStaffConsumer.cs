using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.Enums;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Events.Consumers.CreateStaff;

public class CreateStaffConsumer(
    ILogger<CreateStaffConsumer> _logger,
    IRepository<Staff> _staffRepository,
    IPublishEndpoint _publishEndpoint
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

        var targetAccountId = new AccountId(message.AccountId);
        var existingStaff = _staffRepository.AsQueryable().FirstOrDefault(x => x.AccountId == targetAccountId);
        
        if (existingStaff != null)
        {
            existingStaff.AssignDepartmentAndWarehouse(department, message.WarehouseId);
            await _staffRepository.Update(existingStaff, context.CancellationToken);
            _logger.LogInformation("Updated Staff profile for AccountId: {AccountId}, WarehouseId: {WarehouseId}", message.AccountId, message.WarehouseId);
        }
        else
        {
            var staff = new Staff(
                new StaffId(Guid.NewGuid()),
                targetAccountId,
                Code.Create("STF"),
                message.FullName,
                null, // Phone chưa có
                Email.From(message.Email),
                null, // CitizenId chưa có
                null, // Biography chưa có
                DateTimeOffset.UtcNow,
                department,
                message.WarehouseId
            );
            await _staffRepository.AddAsync(staff, context.CancellationToken);  
            await _publishEndpoint.Publish(new CreateStaffEvent(
            staff.Id.Value,
            staff.AccountId.Value,
            staff.Code,
            staff.Name,
            staff.Email,
            staff.Department,
            staff.CreatedAt
        ), context.CancellationToken);
            _logger.LogInformation("Creating Staff profile for AccountId: {AccountId}, Email: {Email}, WarehouseId: {WarehouseId}", message.AccountId, message.Email, message.WarehouseId);
        }
    }
}

[MessageUrn("staff-created")]
[EntityName("staff-created")]
public sealed record CreateStaffEvent(
    Guid Id,
    Guid AccountId,
    string Code,
    string Name,
    string Email,
    Department Department,
    DateTimeOffset CreatedAt
);

