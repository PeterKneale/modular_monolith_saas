namespace Micro.Common.Domain;

public record OrganisationId(Guid Value)
{
    public static OrganisationId Empty => new(Guid.Empty);
    public static OrganisationId Create() => new(Guid.NewGuid());
    public static OrganisationId Create(Guid guid) => new(guid);
    public static OrganisationId Create(Guid? guid) => guid == null ? Empty : Create(guid.Value);
    public static implicit operator Guid(OrganisationId d) => d.Value;
}