using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken token);
    void Update(User user);
    void Delete(User user);
    Task<User?> GetAsync(UserId id, CancellationToken token);
    Task<User?> GetAsync(EmailAddress email, CancellationToken token);
}