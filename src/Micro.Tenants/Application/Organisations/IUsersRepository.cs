using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Organisations;

public interface IUsersRepository
{
    Task<User?> GetAsync(UserId id, CancellationToken token);
}