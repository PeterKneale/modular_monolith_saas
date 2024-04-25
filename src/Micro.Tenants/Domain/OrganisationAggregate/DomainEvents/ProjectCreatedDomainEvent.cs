namespace Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;

public class ProjectCreatedDomainEvent(OrganisationId organisationId, ProjectId projectId, ProjectName name) : IDomainEvent
{
    public OrganisationId OrganisationId { get; } = organisationId;
    public ProjectId ProjectId { get; } = projectId;
    public ProjectName Name { get; } = name;
}