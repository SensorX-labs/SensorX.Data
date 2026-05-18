using System;
using MassTransit;

namespace SensorX.Data.Application.Commands.Customers.UpdateCustomerAvatar;

[MessageUrn("customer-avatar-updated")]
[EntityName("customer-avatar-updated")]
public sealed record CustomerAvatarUpdatedEvent(
    Guid AccountId,
    string AvatarUrl
);