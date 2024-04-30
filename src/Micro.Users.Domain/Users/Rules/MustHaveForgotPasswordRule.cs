namespace Micro.Users.Domain.Users.Rules;

internal class MustHaveForgotPasswordRule(User user) : IBusinessRule
{
    public string Message => "This action must be performed on a user that has forgotten their password";

    public bool IsBroken() => user.ForgotPasswordToken == null;
}