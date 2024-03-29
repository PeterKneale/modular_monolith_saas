namespace Micro.Users.Domain.Users.Rules;

internal class MustNotBeVerifiedRule(UserVerification verification) : IBusinessRule
{
    public string Message => "This user has already been verified";

    public bool IsBroken() => verification.IsVerified;
}