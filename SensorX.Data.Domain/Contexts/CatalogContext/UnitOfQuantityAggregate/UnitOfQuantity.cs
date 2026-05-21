using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

public class UnitOfQuantity : Entity<UnitOfQuantityId>, IAggregateRoot, ICreationTrackable, IUpdateTrackable
{
    private UnitOfQuantity(UnitOfQuantityId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static UnitOfQuantity Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên đơn vị tính không được để trống");

        return new UnitOfQuantity(UnitOfQuantityId.New(), name.Trim(), description.Trim());
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên đơn vị tính không được để trống");

        Name = name.Trim();
        Description = description.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
