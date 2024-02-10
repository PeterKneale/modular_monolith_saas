using Micro.Common.Domain;

namespace Micro.Tenants.Domain.Memberships;

public class Membership(MembershipId id, OrganisationId organisationId, UserId userId, MembershipRole membershipRole) : BaseEntity
{
    public MembershipId Id { get; } = id;
    public OrganisationId OrganisationId { get; } = organisationId;
    public UserId UserId { get;  } = userId;
    public MembershipRole MembershipRole { get; private set; } = membershipRole;
    
    public void ChangeRole(MembershipRole membershipRole)
    {
        MembershipRole = membershipRole;
    }
}

public record MembershipId(Guid Value);