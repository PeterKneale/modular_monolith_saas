namespace Micro.Tenants.Domain.Users.Rules;

internal class MustBeVerifiedRule(User user) : IBusinessRule
{
    public string Message => "This action must be performed on a verified user";

    public bool IsBroken() => user.Verification.IsVerified == false;
}