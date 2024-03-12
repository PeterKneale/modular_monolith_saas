namespace Micro.Tenants.Domain.Organisations.DomainEvents;

public class OrganisationNameChangedDomainEvent(OrganisationId id, OrganisationName name) : IDomainEvent
{
    public OrganisationId Id { get; } = id;
    public OrganisationName Name { get; } = name;
}