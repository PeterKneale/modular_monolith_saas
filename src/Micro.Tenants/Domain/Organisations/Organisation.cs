using Micro.Tenants.Domain.Memberships;
using Micro.Tenants.Domain.Organisations.DomainEvents;
using Micro.Tenants.Domain.Projects;

namespace Micro.Tenants.Domain.Organisations;

public class Organisation : BaseEntity
{
    private Organisation(OrganisationId id, OrganisationName name)
    {
        Id = id;
        Name = name;
        AddDomainEvent(new OrganisationCreatedDomainEvent(id, name));
    }

    public OrganisationId Id { get; }
    public OrganisationName Name { get; private set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public static Organisation Create(OrganisationId id, OrganisationName name) => new(id, name);

    public void ChangeName(OrganisationName name)
    {
        AddDomainEvent(new OrganisationNameChangedDomainEvent(Id, name));
        Name = name;
    }
}