namespace Micro.Tenants.Domain.Users.Rules;

internal class VerificationTokenMustMatchRule(UserVerification verification, string token) : IBusinessRule
{
    public string Message => "The verification token does not match";

    public bool IsBroken() => !string.Equals(verification.VerificationToken, token, StringComparison.InvariantCultureIgnoreCase);
}