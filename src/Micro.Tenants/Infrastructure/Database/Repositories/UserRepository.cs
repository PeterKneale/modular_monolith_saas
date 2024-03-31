using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class UserRepository(Db db) : IUsersRepository
{
    public async Task<User?> GetAsync(UserId id, CancellationToken token)
    {
        return await db.Users
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }
}