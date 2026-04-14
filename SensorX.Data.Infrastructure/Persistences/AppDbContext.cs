using MediatR;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SensorX.Data.Application.Common.DomainEvent;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Infrastructure.Persistences;

public class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : DbContext(options)
{
    private readonly IMediator _mediator = mediator;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore value object types - they should not be discovered as entities
        modelBuilder.Ignore<AccountId>();
        modelBuilder.Ignore<CitizenId>();
        modelBuilder.Ignore<ProductCategoryId>();
        modelBuilder.Ignore<ProductId>();
        modelBuilder.Ignore<InternalPriceId>();
        modelBuilder.Ignore<CustomerId>();
        modelBuilder.Ignore<ProvinceId>();
        modelBuilder.Ignore<WardId>();
        modelBuilder.Ignore<StaffId>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Count > 0)
                .ToList();

            if (entitiesWithEvents.Count == 0) break;

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    var notification = (INotification)Activator.CreateInstance(
                        typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()),
                        domainEvent
                    )!;

                    await _mediator.Publish(notification, cancellationToken);
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}