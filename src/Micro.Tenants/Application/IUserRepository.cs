using Micro.Common.Domain;
using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetAsync(UserId id);
}