using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public required string Email { get; set; }
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    
    // Shipping Info
    public Guid? WardId { get; set; }
    public string? ShippingAddress { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
}
