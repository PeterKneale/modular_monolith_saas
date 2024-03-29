namespace Micro.Users.Domain.ApiKeys;

public record UserApiKeyId(Guid Value)
{
    public override string ToString() => Value.ToString();
}