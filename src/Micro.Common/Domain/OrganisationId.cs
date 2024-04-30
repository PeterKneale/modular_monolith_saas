namespace Micro.Common.Domain;

public class OrganisationId : IdValueObject
{
    private OrganisationId() : base()
    {
        // efcore
    }

    private OrganisationId(Guid value):base(value)
    {
    }

    public static OrganisationId Create() => new(Guid.NewGuid());
    public static OrganisationId Create(Guid id) => new(id);

    public static implicit operator string(OrganisationId d) => d.Value.ToString();
    public static implicit operator Guid(OrganisationId d) => d.Value;
}