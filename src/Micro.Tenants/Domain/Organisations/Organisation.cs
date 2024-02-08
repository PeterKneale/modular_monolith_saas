using Micro.Common.Domain;

namespace Micro.Tenants.Domain.Organisations;

public class Organisation(OrganisationId id, OrganisationName name) : BaseEntity
{
    public OrganisationId Id { get; } = id;
    public OrganisationName Name { get; private set; } = name;

    public void ChangeName(OrganisationName name)
    {
        Name = name;
    }
}

