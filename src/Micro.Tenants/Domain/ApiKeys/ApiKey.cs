namespace Micro.Tenants.Domain.ApiKeys;

public class ApiKey
{
    private ApiKey(ApiKeyName name, ApiKeyValue key, DateTime createdAt)
    {
        Name = name;
        Key = key;
        CreatedAt = createdAt;
    }

    public ApiKeyName Name { get; }
    
    public ApiKeyValue Key { get; }
    
    public DateTime CreatedAt { get; }

    public bool Match(ApiKey apiKey)
    {
        return apiKey.Key == Key;
    }

    public static ApiKey Create(ApiKeyName name, ApiKeyValue key) => new(name, key, SystemClock.Now);

    public static ApiKey From(ApiKeyName name, ApiKeyValue key, DateTime createdAt) => new(name, key, createdAt);
}