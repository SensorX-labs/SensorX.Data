using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.UserContext;

public abstract class User<UserId> : Entity<UserId>, IAggregateRoot, ICreationTrackable, IUpdateTrackable where UserId : VoId
{
    protected User(UserId id, AccountId accountId, Code code, string name, Phone phone, Email email) : base(id)
    {
        AccountId = accountId;
        Code = code;
        Name = name;
        Phone = phone;
        Email = email;
        CreatedAt = DateTimeOffset.UtcNow;
    }

#pragma warning disable CS8618
    protected User() { }
#pragma warning restore CS8618

    public AccountId AccountId { get; private set; }
    public Code Code { get; private set; }
    public string Name { get; private set; }
    public Phone Phone { get; private set; }
    public Email Email { get; private set; }
    public string AvatarUrl { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected void UpdateProfile(string name, Phone phone, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên không được để trống.");
        Name = name;
        Phone = phone;
        Email = email;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateAvatar(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new DomainException("Avatar URL không được để trống.");
        AvatarUrl = avatarUrl;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
