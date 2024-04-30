namespace Micro.Tenants.Domain.OrganisationAggregate;

public record MembershipId(Guid Value)
{
    public static MembershipId CreateNew() => new(Guid.NewGuid());
}