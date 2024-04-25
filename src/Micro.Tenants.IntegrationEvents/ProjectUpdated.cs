using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.IntegrationEvents;

public class ProjectUpdated : IIntegrationEvent
{
    public Guid ProjectId { get; init; }
    public string ProjectName { get; init; } = null!;
}