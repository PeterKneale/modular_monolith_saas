namespace Micro.Common.Domain;

public record OrganisationId(Guid Value)
{
    public static implicit operator string(OrganisationId d) => d.Value.ToString();
    public static implicit operator Guid(OrganisationId d) => d.Value;
    public override string ToString() => Value.ToString();
}