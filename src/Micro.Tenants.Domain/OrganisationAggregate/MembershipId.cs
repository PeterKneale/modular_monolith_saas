namespace Micro.Tenants.Domain.OrganisationAggregate;

public record MembershipId(Guid Value)
{
    public static MembershipId Create() => new(Guid.NewGuid());
    public static MembershipId Create(Guid guid) => new(guid);
    public static implicit operator Guid(MembershipId d) => d.Value;
}