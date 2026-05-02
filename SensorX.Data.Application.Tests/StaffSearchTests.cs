using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Tests;

public class StaffSearchTests
{
    private static Staff CreateStaff(
        string name,
        string email,
        string? phone = null,
        string? citizenId = null)
    {
        return new Staff(
            StaffId.New(),
            new AccountId(Guid.NewGuid()),
            Code.Create("STF"),
            name,
            phone is not null ? Phone.From(phone) : null,
            Email.From(email),
            citizenId is not null ? CitizenId.From(citizenId) : null,
            biography: null,
            joinDate: DateTimeOffset.UtcNow,
            department: null
        );
    }

    private readonly Staff _staffWithAllFields;
    private readonly Staff _staffWithNullPhone;
    private readonly Staff _staffWithNullCitizenId;
    private readonly Staff _staffWithNullPhoneAndCitizenId;
    private readonly IQueryable<Staff> _allStaff;

    public StaffSearchTests()
    {
        _staffWithAllFields = CreateStaff(
            name: "Nguyen Van A",
            email: "nva@example.com",
            phone: "0912345678",
            citizenId: "012345678901"
        );

        _staffWithNullPhone = CreateStaff(
            name: "Tran Thi B",
            email: "ttb@example.com",
            phone: null,
            citizenId: "098765432109"
        );

        _staffWithNullCitizenId = CreateStaff(
            name: "Le Van C",
            email: "lvc@example.com",
            phone: "0987654321",
            citizenId: null
        );

        _staffWithNullPhoneAndCitizenId = CreateStaff(
            name: "Pham Thi D",
            email: "ptd@example.com",
            phone: null,
            citizenId: null
        );

        _allStaff = new[]
        {
            _staffWithAllFields,
            _staffWithNullPhone,
            _staffWithNullCitizenId,
            _staffWithNullPhoneAndCitizenId
        }.AsQueryable();
    }

    [Fact]
    public void ApplySearch_WhenSearchTermIsNull_ReturnsAllRecords()
    {
        var result = _allStaff.ApplySearch(null).ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void ApplySearch_WhenSearchTermIsEmpty_ReturnsAllRecords()
    {
        var result = _allStaff.ApplySearch("   ").ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void ApplySearch_ByName_ReturnsMatchingStaff()
    {
        var result = _allStaff.ApplySearch("Nguyen Van A").ToList();
        Assert.Single(result);
        Assert.Equal("Nguyen Van A", result[0].Name);
    }

    [Fact]
    public void ApplySearch_ByCode_ReturnsMatchingStaff()
    {
        // All staff share the "STF" prefix in their code, so searching for a partial code
        // that matches all four is consistent. We verify the search does not throw.
        var result = _allStaff.ApplySearch("STF").ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void ApplySearch_ByEmail_ReturnsMatchingStaff()
    {
        var result = _allStaff.ApplySearch("lvc@example.com").ToList();
        Assert.Single(result);
        Assert.Equal("Le Van C", result[0].Name);
    }

    [Fact]
    public void ApplySearch_ByPhone_WhenPhoneIsNotNull_ReturnsMatchingStaff()
    {
        var result = _allStaff.ApplySearch("0912345678").ToList();
        Assert.Single(result);
        Assert.Equal("Nguyen Van A", result[0].Name);
    }

    [Fact]
    public void ApplySearch_ByCitizenId_WhenCitizenIdIsNotNull_ReturnsMatchingStaff()
    {
        var result = _allStaff.ApplySearch("098765432109").ToList();
        Assert.Single(result);
        Assert.Equal("Tran Thi B", result[0].Name);
    }

    [Fact]
    public void ApplySearch_WhenStaffHasNullPhone_DoesNotThrow()
    {
        // StaffWithNullPhone and StaffWithNullPhoneAndCitizenId both have null Phone.
        // Before the fix this would throw NullReferenceException.
        var exception = Record.Exception(() => _allStaff.ApplySearch("example").ToList());
        Assert.Null(exception);
    }

    [Fact]
    public void ApplySearch_WhenStaffHasNullCitizenId_DoesNotThrow()
    {
        // StaffWithNullCitizenId and StaffWithNullPhoneAndCitizenId have null CitizenId.
        // Before the fix this would throw NullReferenceException.
        var exception = Record.Exception(() => _allStaff.ApplySearch("STF").ToList());
        Assert.Null(exception);
    }

    [Fact]
    public void ApplySearch_WithPartialEmail_MatchesAcrossAllRecords()
    {
        // All staff have "@example.com" in their email.
        var result = _allStaff.ApplySearch("@example.com").ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void ApplySearch_WithNoMatch_ReturnsEmpty()
    {
        var result = _allStaff.ApplySearch("nonexistent_term_xyz").ToList();
        Assert.Empty(result);
    }
}
