using Micro.Tenants.Domain.ApiKeys;
using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Domain.Users;

public class User : BaseEntity
{
    private User()
    {
        // ef 
    }

    private User(UserId id, UserName name, UserCredentials credentials)
    {
        Id = id;
        Name = name;
        Credentials = credentials;
    }

    public UserId Id { get; private init; }

    public UserName Name { get; private set; }

    public UserCredentials Credentials { get; private set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<UserApiKey> UserApiKeys { get; set; } = new List<UserApiKey>();

    public static User CreateInstance(UserId id, UserName name, UserCredentials credentials) => new(id, name, credentials);
}