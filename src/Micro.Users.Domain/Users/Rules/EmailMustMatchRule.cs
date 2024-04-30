namespace Micro.Users.Domain.Users.Rules;

public class EmailMustMatchRule(User user, EmailAddress emailAddress) : IBusinessRule
{
    public string Message => "The password is incorrect.";

    public bool IsBroken() => !user.EmailAddress.Matches(emailAddress);
}