using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

public class Ward : Entity<WardId>
{
    public Ward(WardId id, string name, int code) : base(id)
    {
        Name = name;
        Code = code;
    }

    public string Name { get; private set; }
    public int Code { get; private set; }
}