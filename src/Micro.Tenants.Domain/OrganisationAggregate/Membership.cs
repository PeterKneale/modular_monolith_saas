using System.Diagnostics;
using Micro.Tenants.Domain.UserAggregate;

namespace Micro.Tenants.Domain.OrganisationAggregate;

[DebuggerDisplay("{Role.Name} - User:{UserId} - Organisation:{OrganisationId}")]
public class Membership : BaseEntity
{
    private Membership(MembershipId id, OrganisationId organisationId, UserId userId, MembershipRole role)
    {
        Id = id;
        OrganisationId = organisationId;
        UserId = userId;
        Role = role;
        CreatedAt = SystemClock.UtcNow;
    }

    public MembershipId Id { get; private init; }

    public OrganisationId OrganisationId { get; private init; }

    public UserId UserId { get; private init; }

    public MembershipRole Role { get; private set; }
    
    public DateTimeOffset CreatedAt { get;  }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public virtual Organisation Organisation { get; private init; } = null!;

    public virtual User User { get; private init; } = null!;

    public static Membership CreateMember(OrganisationId organisationId, UserId userId) =>
        new(MembershipId.Create(), organisationId, userId, MembershipRole.Member);

    public static Membership CreateOwner(OrganisationId organisationId, UserId userId) =>
        new(MembershipId.Create(), organisationId, userId, MembershipRole.Owner);

    public void SetRole(MembershipRole role)
    {
        Role = role;
        UpdatedAt = SystemClock.UtcNow;
    }

}