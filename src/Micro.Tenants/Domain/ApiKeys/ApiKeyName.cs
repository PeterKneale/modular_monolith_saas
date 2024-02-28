namespace Micro.Tenants.Domain.ApiKeys;

public record ApiKeyName(string Name)
{
    public override string ToString() => $"{Name}";
}