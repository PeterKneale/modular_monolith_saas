using Micro.Users.Application.Users;
using Micro.Users.Domain.Users;

namespace Micro.Users.Infrastructure.Infrastructure.Database.Repositories;

internal class UserRepository(Db db) : IUserRepository
{
    public async Task CreateAsync(User user, CancellationToken token)
    {
        await db.Users.AddAsync(user, token);
    }

    public void Update(User user) => db.Users.Update(user);

    public void Delete(User user) => db.Users.Remove(user);

    public async Task<User?> GetAsync(UserId id, CancellationToken token)
    {
        return await db.Users
            .Include(x => x.UserApiKeys)
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<User?> GetAsync(EmailAddress emailAddress, CancellationToken token)
    {
        return await db.Users
            .Include(x => x.UserApiKeys)
            .SingleOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress), token);
    }
}