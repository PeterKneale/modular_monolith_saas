namespace Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;

public class ProjectUpdatedDomainEvent(ProjectId projectId, ProjectName name) : IDomainEvent
{
    public ProjectId ProjectId { get; } = projectId;
    public ProjectName Name { get; } = name;
}