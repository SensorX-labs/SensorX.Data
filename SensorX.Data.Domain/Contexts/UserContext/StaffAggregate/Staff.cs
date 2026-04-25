using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

public class Staff : User<StaffId>
{
    public Staff(
        StaffId id,
        AccountId accountId,
        Code code,
        string name,
        Phone? phone,
        Email email,
        CitizenId? citizenId,
        string? biography,
        DateTimeOffset joinDate,
        Department? department
    ) : base(id, accountId, code, name, phone, email)
    {
        CitizenId = citizenId;
        Biography = biography;
        JoinDate = joinDate;
        Department = department;
    }

    public CitizenId? CitizenId { get; private set; }
    public string? Biography { get; private set; }
    public DateTimeOffset JoinDate { get; private set; }
    public Department? Department { get; private set; }

    public void UpdateProfile(
        string name,
        Phone? phone,
        Email email,
        CitizenId? citizenId,
        string? biography,
        DateTimeOffset joinDate,
        Department? department
    )
    {
        base.UpdateProfile(name, phone, email);
        CitizenId = citizenId;
        Biography = biography;
        JoinDate = joinDate;
        Department = department;
    }
}