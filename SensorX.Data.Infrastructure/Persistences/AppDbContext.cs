using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SensorX.Data.Application.Common.DomainEvent;
using SensorX.Data.Domain.SeedWork;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        // Fix for PostgreSQL DateTimeOffset with offset issue
        // Npgsql 6.0+ requires DateTimeOffset to be in UTC (offset 0) when saving to 'timestamp with time zone'
        var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, DateTimeOffset>(
            v => v.ToUniversalTime(),
            v => v);

        var nullableDateTimeOffsetConverter = new ValueConverter<DateTimeOffset?, DateTimeOffset?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(dateTimeOffsetConverter);
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(nullableDateTimeOffsetConverter);
                }
            }
        }
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