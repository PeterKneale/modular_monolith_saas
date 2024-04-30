using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.Messages;

public class ProjectCreated : IIntegrationEvent
{
    public Guid OrganisationId { get; init; }
    public Guid ProjectId { get; init; }
    public string ProjectName { get; init; } = null!;
}