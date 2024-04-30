namespace Micro.Common.Domain;

public record OrganisationId(Guid Value)
{
    public static OrganisationId Create() => new(Guid.NewGuid());
    public static OrganisationId Create(Guid guid) => new(guid);
    public static implicit operator Guid(OrganisationId d) => d.Value;
}