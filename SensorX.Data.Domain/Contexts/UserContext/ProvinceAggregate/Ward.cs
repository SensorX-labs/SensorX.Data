using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public class Ward : Entity<WardId>
{
    private Ward() : base() { }
    public Ward(WardId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
}