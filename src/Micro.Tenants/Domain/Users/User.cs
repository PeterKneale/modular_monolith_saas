using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Domain.Users;

public class User
{
    public UserId Id { get; init; } = null!;

    public string Name { get; set; } = null!;
    
    public virtual ICollection<Membership> Memberships { get; private init; } = new List<Membership>();
}