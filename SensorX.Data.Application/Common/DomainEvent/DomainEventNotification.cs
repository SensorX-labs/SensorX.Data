using SensorX.Data.Domain.SeedWork;
using MediatR;

namespace SensorX.Data.Application.Common.DomainEvent;
public record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent): INotification where TDomainEvent : IDomainEvent;

