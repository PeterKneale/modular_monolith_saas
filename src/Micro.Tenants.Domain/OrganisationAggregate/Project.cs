namespace Micro.Tenants.Domain.OrganisationAggregate;

public class Project(ProjectId projectId, OrganisationId organisationId, ProjectName name) : BaseEntity
{
    public ProjectId ProjectId { get; } = projectId;
    public OrganisationId OrganisationId { get; } = organisationId;
    public ProjectName Name { get; private set; } = name;

    public DateTimeOffset CreatedAt { get; } = SystemClock.UtcNow;
    public DateTimeOffset? UpdatedAt { get; private set; }

    public virtual Organisation Organisation { get; set; } = null!;

    public void ChangeName(ProjectName name)
    {
        Name = name;
        UpdatedAt = SystemClock.UtcNow;
    }
}