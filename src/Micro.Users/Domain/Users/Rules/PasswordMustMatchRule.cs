using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Domain.Users.Rules;

public class PasswordMustMatchRule(User user, Password password, ICheckPassword checker) : IBusinessRule
{
    public string Message => "The password is incorrect.";

    public bool IsBroken()
    {
        return !checker.Matches(password, user.HashedPassword);
    }
}