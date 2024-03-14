using Micro.Tenants.Domain.Users.Rules;

namespace Micro.Tenants.Domain.Users;

public class UserVerification : BaseEntity
{
    private UserVerification()
    {
        // ef
    }

    private UserVerification(bool isVerified, string? verificationToken)
    {
        IsVerified = isVerified;
        VerifiedAt = null;
        VerificationToken = verificationToken;
    }

    public bool IsVerified { get; private set; }

    public DateTimeOffset? VerifiedAt { get; private set; }

    public string? VerificationToken { get; private set; }

    public static UserVerification Unverified() => new(false, Guid.NewGuid().ToString());

    public void Verify(string token)
    {
        CheckRule(new MustNotBeVerifiedRule(this));
        CheckRule(new VerificationTokenMustMatchRule(this, token));
        IsVerified = true;
        VerifiedAt = SystemClock.Now;
        VerificationToken = null;
    }
}