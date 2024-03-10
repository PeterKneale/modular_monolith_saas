using Micro.Tenants.Application.Users;
using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Domain.ApiKeys;

public class UserApiKey
{
    private UserApiKey()
    {
        // ef core
    }

    private UserApiKey(UserApiKeyId id, UserId userId, ApiKey apiKey)
    {
        Id = id;
        UserId = userId;
        ApiKey = apiKey;
    }

    public UserApiKeyId Id { get; }

    public UserId UserId { get; }

    public ApiKey ApiKey { get; }

    public virtual User User { get; set; } = null!;

    public override string ToString() => $"{Id} - {UserId} - {ApiKey}";

    public static UserApiKey CreateNew(UserApiKeyId id, UserId userId, ApiKeyName name, IApiKeyService service) => new(id, userId, ApiKey.Create(name, service.GenerateApiKey()));

    public static UserApiKey Create(UserApiKeyId id, UserId userId, ApiKey key) => new(id, userId, key);
}