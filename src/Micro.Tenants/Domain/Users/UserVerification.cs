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

    public static UserVerification Unverified() => new(false, Guid.NewGuid().ToString());
    
    public void Verify(string token)
    {
        if (IsVerified)
        {
            throw new BusinessRuleBrokenException("User is already verified.");
        }
        if (token != VerificationToken)
        {
            throw new BusinessRuleBrokenException("Invalid verification token.");
        }
        IsVerified = true;
        VerifiedAt = SystemClock.Now;
        VerificationToken = null;
    }

    public bool IsVerified { get; private set; }
    
    public DateTimeOffset? VerifiedAt { get; private set; }
    
    public string? VerificationToken { get; private set; }
}