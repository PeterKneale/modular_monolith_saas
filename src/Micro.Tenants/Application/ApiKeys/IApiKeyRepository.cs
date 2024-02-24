using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys;

public interface IApiKeyRepository
{
    Task CreateAsync(UserApiKey key, CancellationToken token);
    Task<UserApiKey?> GetAsync(UserApiKeyId id, CancellationToken token);
    Task<UserApiKey?> GetAsync(UserId userId, ApiKeyName name, CancellationToken token);
    Task<UserApiKey?> GetAsync(ApiKeyValue key, CancellationToken token);
}