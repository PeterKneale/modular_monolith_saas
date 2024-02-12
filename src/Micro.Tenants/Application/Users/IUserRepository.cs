using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken token);
    Task<User?> GetAsync(UserId id, CancellationToken token);
    Task<User?> GetAsync(string email, CancellationToken token);
}