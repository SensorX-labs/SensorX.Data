using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public class Province : Entity<ProvinceId>, IAggregateRoot
{
    public Province(ProvinceId id, string name, int code) : base(id)
    {
        Name = name;
        Code = code;
    }

    public string Name { get; private set; }
    public int Code { get; private set; }

    private readonly List<Ward> _wards = [];
    public IReadOnlyList<Ward> Wards => _wards.AsReadOnly();

    public void AddWard(string name, int code)
    {
        _wards.Add(new Ward(WardId.New(), name, code));
    }
}