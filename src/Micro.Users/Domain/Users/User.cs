using Micro.Users.Domain.Users.DomainEvents;
using Micro.Users.Domain.Users.Rules;

namespace Micro.Users.Domain.Users;

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
        AddDomainEvent(new UserCreatedDomainEvent(this));
        Verification = UserVerification.Unverified();
    }

    public UserId Id { get; } = null!;

    public UserName Name { get; private set; } = null!;

    public UserCredentials Credentials { get; } = null!;

    public UserVerification Verification { get; } = null!;

    public virtual ICollection<UserApiKey> UserApiKeys { get; set; } = new List<UserApiKey>();

    public static User CreateInstance(UserId id, UserName name, UserCredentials credentials) =>
        new(id, name, credentials);

    public bool CanLogin(UserCredentials credentials) => Verification.IsVerified && Credentials.Matches(credentials);

    public void ChangeName(UserName name)
    {
        AddDomainEvent(new UserNameChangedDomainEvent(Id, name));
        Name = name;
    }

    public void ChangePassword(Password oldPassword, Password newPassword)
    {
        CheckRule(new MustBeVerifiedRule(Verification));
        CheckRule(new PasswordMatchesRule(this, oldPassword));
        Credentials.ChangePassword(newPassword);
    }
}