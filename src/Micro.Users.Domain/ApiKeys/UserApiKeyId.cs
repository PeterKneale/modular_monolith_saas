namespace Micro.Users.Domain.ApiKeys;

public record UserApiKeyId(Guid Value)
{
    public static UserApiKeyId Create() => new(Guid.NewGuid());
    public static UserApiKeyId Create(Guid guid) => new(guid);
    public static implicit operator Guid(UserApiKeyId d) => d.Value;
}