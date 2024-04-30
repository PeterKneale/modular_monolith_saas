using Micro.Users.Application.ApiKeys;
using Micro.Users.Domain.ApiKeys;

namespace Micro.Users.Infrastructure.Database.Repositories;

internal class ApiKeyRepository(Db db) : IApiKeyRepository
{
    public async Task CreateAsync(UserApiKey key, CancellationToken token)
    {
        await db.UserApiKeys.AddAsync(key, token);
    }

    public void Delete(UserApiKey key)
    {
        db.UserApiKeys.Remove(key);
    }

    public async Task<UserApiKey?> GetById(UserApiKeyId id, CancellationToken token)
    {
        return await db.UserApiKeys
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<UserApiKey?> GetByName(UserId userId, ApiKeyName name, CancellationToken token)
    {
        return await db.UserApiKeys
            .SingleOrDefaultAsync(x => x.UserId == userId && x.ApiKey.Name == name, token);
    }

    public async Task<UserApiKey?> GetByKey(ApiKeyValue key, CancellationToken token)
    {
        return await db.UserApiKeys
            .SingleOrDefaultAsync(x => x.ApiKey.Key == key, token);
    }

    public async Task<IEnumerable<UserApiKey>> ListAsync(UserId userId, CancellationToken token)
    {
        return await db.UserApiKeys
            .Where(x => x.UserId == userId)
            .ToListAsync(token);
    }
}