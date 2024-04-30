namespace Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;

public class OrganisationCreatedDomainEvent(OrganisationId id, OrganisationName name) : IDomainEvent
{
    public OrganisationId Id { get; } = id;
    public OrganisationName Name { get; } = name;
}