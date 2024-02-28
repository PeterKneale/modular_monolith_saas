using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys;

public interface IApiKeyRepository
{
    Task CreateAsync(UserApiKey key, CancellationToken token);
    Task DeleteAsync(UserApiKeyId id, CancellationToken token);
    Task<UserApiKey?> GetById(UserApiKeyId id, CancellationToken token);
    Task<UserApiKey?> GetByName(UserId userId, ApiKeyName name, CancellationToken token);
    Task<UserApiKey?> GetByKey(ApiKeyValue key, CancellationToken token);
    Task<IEnumerable<UserApiKey>> ListAsync(UserId userId, CancellationToken token);
}