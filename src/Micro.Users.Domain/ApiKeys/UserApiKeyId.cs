namespace Micro.Users.Domain.ApiKeys;

public class UserApiKeyId : IdValueObject
{
    private UserApiKeyId(Guid value) : base(value)
    {
    }

    public static UserApiKeyId Create() => new(Guid.NewGuid());
    public static UserApiKeyId Create(Guid id) => new(id);

    public static implicit operator string(UserApiKeyId d) => d.Value.ToString();
    public static implicit operator Guid(UserApiKeyId d) => d.Value;
}