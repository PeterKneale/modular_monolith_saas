using Micro.Users.Domain.ApiKeys;
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

    private User(UserId id, Name name, EmailAddress emailAddress, HashedPassword hashedPassword, string verificationToken)
    {
        Id = id;
        Name = name;
        EmailAddress = emailAddress;
        HashedPassword = hashedPassword;
        VerificationToken = verificationToken;
        RegisteredAt = SystemClock.UtcNow;
        AddDomainEvent(new UserCreatedDomainEvent(this));
    }

    public UserId Id { get; } = null!;

    public Name Name { get; private set; } = null!;

    public EmailAddress EmailAddress { get; } = null!;

    public HashedPassword HashedPassword { get; private set; } = null!;

    public DateTimeOffset RegisteredAt { get; private set; }

    public bool IsVerified { get; private set; }

    public DateTimeOffset? VerifiedAt { get; private set; }

    public string? VerificationToken { get; private set; }
    
    public string? ForgotPasswordToken { get; set; }

    public DateTimeOffset? ForgotPasswordTokenExpiry { get; set; }

    public virtual ICollection<UserApiKey> UserApiKeys { get; set; } = new List<UserApiKey>();

    public static User Create(UserId id, Name name, EmailAddress emailAddress, Password password, IHashPassword hasher)
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
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new EmailMustMatchRule(this, emailAddress));
        CheckRule(new PasswordMustMatchRule(this, password, checker));
    }

    public void ChangeName(Name name)
    {
        AddDomainEvent(new UserNameChangedDomainEvent(Id, name));
        Name = name;
    }

    public void ChangePassword(Password oldPassword, Password newPassword, ICheckPassword checker, IHashPassword hasher)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new PasswordMustMatchRule(this, oldPassword, checker));
        var newPasswordHashed = hasher.HashPassword(newPassword);
        HashedPassword = newPasswordHashed;
    }
    
    public void ForgotPassword()
    {
        CheckRule(new MustBeVerifiedRule(this));
        ForgotPasswordToken = Guid.NewGuid().ToString();
        ForgotPasswordTokenExpiry = SystemClock.UtcNow + TimeSpan.FromHours(24);
    }
    
    public void ResetPassword(string token, Password password, IHashPassword hasher)
    {
        CheckRule(new MustBeVerifiedRule(this));
        CheckRule(new MustHaveForgotPasswordRule(this));
        CheckRule(new ForgotTokenMustMatchRule(this, token));
        CheckRule(new ForgotTokenMustNotBeExpiredRule(this));
        ForgotPasswordToken = null;
        ForgotPasswordTokenExpiry = null;
        HashedPassword = hasher.HashPassword(password);
    }
}