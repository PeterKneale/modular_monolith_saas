using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.IntegrationEvents;

public class OrganisationCreated : IntegrationEvent
{
    public Guid OrganisationId { get; init; }
    public string OrganisationName { get; init; } = null!;
}