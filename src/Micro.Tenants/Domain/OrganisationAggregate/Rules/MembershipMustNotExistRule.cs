namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class MembershipMustNotExistRule(IEnumerable<Membership> memberships, UserId id) : IBusinessRule
{
    public string Message => "User is already has a membership";

    public bool IsBroken() => memberships.Any(x => x.UserId.Equals(id));
}