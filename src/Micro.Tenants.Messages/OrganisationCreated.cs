using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.Messages;

public class OrganisationCreated : IIntegrationEvent
{
    public Guid OrganisationId { get; init; }
    public string OrganisationName { get; init; } = null!;
}