using Micro.Common.Domain;
using Micro.Tenants.Domain.Organisations;

namespace Micro.Tenants.Domain.Users;

public class User(OrganisationId organisationId, UserId id, UserName name, UserCredentials credentials, UserRole role)
{
    public OrganisationId OrganisationId { get; } = organisationId;
    public UserId Id { get; } = id;
    public UserName Name { get; } = name;
    public UserCredentials Credentials { get; set; } = credentials;
    public UserRole Role { get; } = role;
}