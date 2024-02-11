using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Domain.Users;

public class User(UserId id, UserName name, UserCredentials credentials)
{
    public UserId Id { get; } = id;
    public UserName Name { get; } = name;
    public UserCredentials Credentials { get; set; } = credentials;
}