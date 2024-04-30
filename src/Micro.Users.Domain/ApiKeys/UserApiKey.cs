using Micro.Users.Domain.ApiKeys.Services;
using Micro.Users.Domain.Users;

namespace Micro.Users.Domain.ApiKeys;

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

    public UserApiKeyId Id { get; } = null!;

    public UserId UserId { get; } = null!;

    public ApiKey ApiKey { get; } = null!;

    public virtual User User { get; set; } = null!;

    public override string ToString() => $"{Id} - {UserId} - {ApiKey}";

    public static UserApiKey CreateNew(UserApiKeyId id, UserId userId, ApiKeyName name, IApiKeyService service) => new(id, userId, ApiKey.Create(name, service.GenerateApiKey()));

    public static UserApiKey Create(UserApiKeyId id, UserId userId, ApiKey key) => new(id, userId, key);
}