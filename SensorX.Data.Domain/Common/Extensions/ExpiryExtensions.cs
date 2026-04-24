using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Common.Extensions;

public static class ExpiryExtensions
{
    // Marks the entity to expire after a specified duration
    public static void MarkExpiredIn(this IExpirable entity, TimeSpan duration)
    {
        entity.ExpiresAt = DateTimeOffset.UtcNow.Add(duration);
    }

    // Marks the entity to expire immediately
    public static void MarkExpiredNow(this IExpirable entity)
    {
        entity.ExpiresAt = DateTimeOffset.UtcNow;
    }

    // Marks the entity as having no expiration (infinite)
    public static void MarkInfinite(this IExpirable entity)
    {
        entity.ExpiresAt = DateTimeOffset.MaxValue;
    }

    // Extends the expiry by a specified duration
    public static void ExtendExpiry(this IExpirable entity, TimeSpan duration)
    {
        entity.ExpiresAt = entity.ExpiresAt.Add(duration);
    }

    // Sets the expiry to a specific date and time
    public static void ExtendExpiryAt(this IExpirable entity, DateTimeOffset expiredAt)
    {
        entity.ExpiresAt = expiredAt;
    }

    // Checks if the entity has expired
    public static bool IsExpired(this IExpirable entity)
        => entity.ExpiresAt <= DateTimeOffset.UtcNow;
}

