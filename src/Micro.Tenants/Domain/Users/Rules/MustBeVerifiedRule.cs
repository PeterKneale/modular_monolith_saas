namespace Micro.Tenants.Domain.Users.Rules;

internal class MustBeVerifiedRule(UserVerification verification) : IBusinessRule
{
    public string Message => "This action must be performed on a verified user";

    public bool IsBroken() => verification.IsVerified == false;
}