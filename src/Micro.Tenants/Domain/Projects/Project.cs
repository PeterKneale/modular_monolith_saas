namespace Micro.Tenants.Domain.Projects;

public class Project(ProjectId id, OrganisationId organisationId, ProjectName name) : BaseEntity
{
    public ProjectId Id { get; } = id;
    public OrganisationId OrganisationId { get; } = organisationId;
    public ProjectName Name { get; private set; } = name;

    public void ChangeName(ProjectName name)
    {
        Name = name;
    }
}