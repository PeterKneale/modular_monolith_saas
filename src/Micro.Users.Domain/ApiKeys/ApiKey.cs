namespace Micro.Users.Domain.ApiKeys;

public class ApiKey
{
    private ApiKey(ApiKeyName name, ApiKeyValue key)
    {
        Name = name;
        Key = key;
        CreatedAt = SystemClock.UtcNow;
    }

    public ApiKeyName Name { get; }

    public ApiKeyValue Key { get; }

    public DateTimeOffset CreatedAt { get; }

    public override string ToString() => $"{Name}:{Key}";

    public static ApiKey Create(ApiKeyName name, ApiKeyValue key)
        => new(name, key);
}