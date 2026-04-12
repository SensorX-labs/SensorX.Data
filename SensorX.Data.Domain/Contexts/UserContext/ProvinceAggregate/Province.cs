using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public class Province : Entity<ProvinceId>, IAggregateRoot
{
    private Province() : base() { }
    public Province(ProvinceId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
    private readonly List<Ward> _wards = [];
    public IReadOnlyList<Ward> Wards => _wards.AsReadOnly();

    public void AddWard(Ward ward)
    {
        if (ward is null)
            throw new DomainException("Ward không được để trống.");
        _wards.Add(ward);
    }
}