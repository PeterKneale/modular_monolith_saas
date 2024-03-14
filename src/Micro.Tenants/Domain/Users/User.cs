using Micro.Tenants.Domain.ApiKeys;
using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Users.DomainEvents;
using Micro.Tenants.Domain.Users.Rules;

namespace Micro.Tenants.Domain.Users;

public class User : BaseEntity
{
    private User()
    {
        // ef 
    }

    private User(UserId id, UserName name, UserCredentials credentials)
    {
        Id = id;
        Name = name;
        Credentials = credentials;
        AddDomainEvent(new UserCreatedDomainEvent(id, name));
        Verification = UserVerification.Unverified();
    }

    public UserId Id { get; private init; } = null!;

    public UserName Name { get; private set; } = null!;

    public UserCredentials Credentials { get; private set; } = null!;

    public UserVerification Verification { get; private init; } = null!;

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<UserApiKey> UserApiKeys { get; set; } = new List<UserApiKey>();

    public static User CreateInstance(UserId id, UserName name, UserCredentials credentials) =>
        new(id, name, credentials);

    public bool CanLogin(UserCredentials credentials)
    {
        CheckRule(new MustBeVerifiedRule(this));
        return Credentials.Matches(credentials);
    }
    
    public void ChangeName(UserName name)
    {
        AddDomainEvent(new UserNameChangedDomainEvent(Id, name));
        Name = name;
    }

    public void ChangePassword(Password oldPassword, Password newPassword)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new PasswordMatchesRule(this, oldPassword));
        Credentials.ChangePassword(newPassword);
    }
}