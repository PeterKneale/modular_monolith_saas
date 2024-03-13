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

    public MembershipId Id { get; private init; } = null!;

    public OrganisationId OrganisationId { get; private init; } = null!;

    public UserId UserId { get; private init; } = null!;

    public MembershipRole Role { get; private set; } = null!;

    public virtual Organisation Organisation { get; private init; } = null!;

    public virtual User User { get; private init; } = null!;

    public static Membership CreateInstance(MembershipId id, OrganisationId organisationId, UserId userId, MembershipRole membershipRole) =>
        new(id, organisationId, userId, membershipRole);

    public void SetRole(MembershipRole role)
    {
        Role = role;
    }
}