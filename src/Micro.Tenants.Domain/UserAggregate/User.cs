using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.Domain.UserAggregate;

public class User
{
    public UserId Id { get; init; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Membership> Memberships { get; private init; } = new List<Membership>();
}