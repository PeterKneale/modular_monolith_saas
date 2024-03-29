namespace Micro.Users.Domain.Users.Rules;

internal class PasswordMatchesRule(User user, Password password) : IBusinessRule
{
    public string Message => "The old password does not match";

    public bool IsBroken() => !user.Credentials.Password.Matches(password);
}