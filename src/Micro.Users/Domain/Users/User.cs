using Micro.Users.Domain.Users.DomainEvents;
using Micro.Users.Domain.Users.Rules;
using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Domain.Users;

public class User : BaseEntity
{
    private User()
    {
        // ef 
    }

    private User(UserId id, UserName name, EmailAddress emailAddress, HashedPassword hashedPassword, string verificationToken)
    {
        Id = id;
        Name = name;
        EmailAddress = emailAddress;
        HashedPassword = hashedPassword;
        VerificationToken = verificationToken;
        AddDomainEvent(new UserCreatedDomainEvent(this));
    }

    public UserId Id { get; } = null!;

    public UserName Name { get; private set; } = null!;

    public EmailAddress EmailAddress { get; } = null!;

    public HashedPassword HashedPassword { get; private set; } = null!;

    public bool IsVerified { get; private set; }

    public DateTimeOffset? VerifiedAt { get; private set; }

    public string? VerificationToken { get; private set; }

    public virtual ICollection<UserApiKey> UserApiKeys { get; set; } = new List<UserApiKey>();

    public static User Create(UserId id, UserName name, EmailAddress emailAddress, Password password, IHashPassword hasher)
    {
        var hashedPassword = hasher.HashPassword(password);
        var verificationToken = Guid.NewGuid().ToString();
        return new(id, name, emailAddress, hashedPassword, verificationToken);
    }
    
    public void Verify(string token)
    {
        CheckRule(new MustNotBeVerifiedRule(this));
        CheckRule(new VerificationTokenMustMatchRule(this, token));
        IsVerified = true;
        VerifiedAt = SystemClock.UtcNow;
        VerificationToken = null;
    }

    public void Login(EmailAddress emailAddress, Password password, ICheckPassword checker)
    {
        CheckRule(new MustBeVerified(this));
        CheckRule(new EmailMustMatch(this, emailAddress));
        CheckRule(new PasswordMustMatch(this, password, checker));
    }

    public void ChangeName(UserName name)
    {
        AddDomainEvent(new UserNameChangedDomainEvent(Id, name));
        Name = name;
    }

    public void ChangePassword(Password oldPassword, Password newPassword, ICheckPassword checker, IHashPassword hasher)
    {
        CheckRule(new MustBeVerified(this));
        CheckRule(new PasswordMustMatch(this, oldPassword, checker));
        var newPasswordHashed = hasher.HashPassword(newPassword);
        HashedPassword = newPasswordHashed;
    }
}