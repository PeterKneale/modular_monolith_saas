namespace Micro.Users.Domain.Users.Rules;

internal class VerificationTokenMustMatchRule(User user, string token) : IBusinessRule
{
    public string Message => "The verification token does not match";

    public bool IsBroken() => !string.Equals(user.VerificationToken, token, StringComparison.InvariantCultureIgnoreCase);
}