using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

public class Staff(
    StaffId id,
    AccountId accountId,
    Code code,
    string name,
    Phone? phone,
    Email email,
    CitizenId? citizenId,
    string? biography,
    DateTimeOffset joinDate,
    Department department
    ) : User<StaffId>(id, accountId, code, name, phone, email)
{
    public CitizenId? CitizenId { get; private set; } = citizenId;
    public string? Biography { get; private set; } = biography;
    public DateTimeOffset JoinDate { get; private set; } = joinDate;
    public Department Department { get; private set; } = department;

    public void UpdateProfile(
        string name,
        Phone? phone,
        Email email,
        CitizenId? citizenId,
        string? biography,
        DateTimeOffset joinDate,
        Department department
    )
    {
        base.UpdateProfile(name, phone, email);
        CitizenId = citizenId;
        Biography = biography;
        JoinDate = joinDate;
        Department = department;
    }

    public void AssignDepartmentAndWarehouse(Department department, Guid? warehouseId)
    {
        Department = department;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}