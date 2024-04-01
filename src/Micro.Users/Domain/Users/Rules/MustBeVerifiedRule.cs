namespace Micro.Users.Domain.Users.Rules;

internal class MustBeVerified(User user) : IBusinessRule
{
    public string Message => "This action must be performed on a verified user";

    public bool IsBroken() => user.IsVerified == false;
}