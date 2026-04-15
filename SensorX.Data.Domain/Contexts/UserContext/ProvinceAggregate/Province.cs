using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public class Province : Entity<ProvinceId>, IAggregateRoot
{
    public Province(ProvinceId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
}