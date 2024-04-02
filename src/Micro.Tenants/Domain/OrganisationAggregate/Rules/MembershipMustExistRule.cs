namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class MembershipMustExistRule(IEnumerable<Membership> memberships, UserId id) : IBusinessRule
{
    public string Message => "User does not have a membership";

    public bool IsBroken() => !memberships.Any(x => x.UserId.Equals(id));
}