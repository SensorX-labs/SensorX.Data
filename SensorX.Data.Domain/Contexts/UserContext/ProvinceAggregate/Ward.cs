using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public class Ward : Entity<WardId>, IAggregateRoot
{
    public Ward(WardId id, string name, ProvinceId provinceId) : base(id)
    {
        Name = name;
        ProvinceId = provinceId;
    }

    public string Name { get; private set; }
    public ProvinceId ProvinceId { get; private set; }
    public Province Province { get; private set; }
}