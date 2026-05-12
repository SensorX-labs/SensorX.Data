using MassTransit;
namespace SensorX.Data.Application.Commands.Customers.UpdateShippingInfo;

[MessageUrn("customer-shipping-updated")]
[EntityName("customer-shipping-updated")]
public sealed record UpdateShippingInfoEvent(
    Guid Id,
    string? ShippingAddress,
    string? ReceiverName,
    string? ReceiverPhone
);