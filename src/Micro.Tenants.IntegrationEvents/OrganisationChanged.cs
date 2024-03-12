using Micro.Common.Infrastructure.Outbox;

namespace Micro.Tenants.IntegrationEvents;

public class OrganisationChanged : IntegrationEvent
{
    public Guid OrganisationId { get; init; }
    public string OrganisationName { get; init; }
}