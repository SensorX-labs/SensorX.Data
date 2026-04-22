using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands;

public class CreateCustomerCommand : IRequest<Result<Guid>>
{
    public Guid AccountId { get; set; }
    public required string Name { get; set; }
    public required string TaxCode { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }

    // Shipping Info 
    public Guid? WardId { get; set; }
    public string? ShippingAddress { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
}
