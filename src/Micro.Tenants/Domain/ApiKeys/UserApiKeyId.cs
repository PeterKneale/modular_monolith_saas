namespace Micro.Tenants.Domain.ApiKeys;

public record UserApiKeyId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
