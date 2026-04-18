using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands;

public class CreateCustomerCommand : IRequest<Result<Guid>>
{
    public Guid AccountId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string TaxCode { get; set; }
    public string Address { get; set; }
    public Guid WardId { get; set; }
    public string ShippingAddress { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverPhone { get; set; }
}
