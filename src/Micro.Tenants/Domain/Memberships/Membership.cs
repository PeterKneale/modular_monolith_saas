using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Domain.Memberships;

public class Membership : BaseEntity
{
    private Membership()
    {
        // ef core
    }
    
    private Membership(MembershipId id, OrganisationId organisationId, UserId userId, MembershipRole role)
    {
        Id = id;
        OrganisationId = organisationId;
        UserId = userId;
        Role = role;
    }

    public static Membership CreateInstance(MembershipId id, OrganisationId organisationId, UserId userId, MembershipRole membershipRole)
    {
        return new Membership(id, organisationId, userId, membershipRole);
    }

    public MembershipId Id { get;private init; }
    public OrganisationId OrganisationId { get;private init; }
    public UserId UserId { get; private init; }
    public MembershipRole Role { get; private set; }

    public void ChangeRole(MembershipRole membershipRole)
    {
        Role = membershipRole;
    }
    
    public virtual Organisation Organisation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}