namespace Micro.Tenants.Domain.OrganisationAggregate.Rules;

public class MembershipMustBeForRole(List<Membership> memberships, UserId userId, MembershipRole role) : IBusinessRule
{
    public string Message => $"Membership must be for role {role.Name}";

    public bool IsBroken()
    {
        foreach (var membership in memberships)
        {
            // find membership for user
            if (membership.UserId.Equals(userId))
            {
                // the rule is broken if the role is not the same
                return !membership.Role.Equals(role);
            }
        }

        throw new NotSupportedException("No membership is present");
    }
}