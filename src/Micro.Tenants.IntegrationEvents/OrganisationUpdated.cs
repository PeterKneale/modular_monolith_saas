using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.IntegrationEvents;

public class OrganisationUpdated : IIntegrationEvent
{
    public Guid OrganisationId { get; init; }
    public string OrganisationName { get; init; } = null!;
}