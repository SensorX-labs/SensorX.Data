using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;

public class Supplier : Entity<SupplierId>, IAggregateRoot, ICreationTrackable, IUpdateTrackable
{
    private Supplier(SupplierId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Supplier Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên nhà cung cấp không được để trống");

        return new Supplier(SupplierId.New(), name.Trim(), description.Trim());
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên nhà cung cấp không được để trống");

        Name = name.Trim();
        Description = description.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
