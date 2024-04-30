namespace Micro.Users.Domain.Users.Rules;

internal class ForgotTokenMustNotBeExpiredRule(User user) : IBusinessRule
{
    public string Message => "The forgot token has expired";

    public bool IsBroken() => SystemClock.UtcNow > user.ForgotPasswordTokenExpiry;
}