namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class OtherOwnersMustExistRule(IEnumerable<Membership> memberships, UserId ownerId) : IBusinessRule
{
    public string Message => "Organisation must have at least one owner";

    public bool IsBroken()
    {
        foreach (var membership in memberships)
        {
            // skip this owner
            if (membership.UserId.Equals(ownerId))
            {
                continue;
            }

            // attempt to find another owner
            if (membership.Role.Equals(MembershipRole.Owner))
            {
                // another owner is found
                return false;
            }
        }

        // the rule is broken if no other owner is found
        return true;
    }
}