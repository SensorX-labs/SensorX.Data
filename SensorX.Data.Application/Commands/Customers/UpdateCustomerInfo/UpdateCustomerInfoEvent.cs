using MassTransit;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerInfo;

[MessageUrn("Customer-Updated-Info-Event")]
[EntityName("Customer-Updated-Info-Event")]
public sealed record UpdateCustomerInfoEvent(
    Guid Id,
    string Name,
    string? Phone,
    string Email,
    string TaxCode,
    string? Address
);

