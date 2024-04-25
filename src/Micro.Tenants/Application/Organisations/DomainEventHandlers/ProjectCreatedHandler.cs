using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class ProjectCreatedHandler(OutboxWriter outbox, ILogger<ProjectCreatedHandler> logs) : INotificationHandler<ProjectCreatedDomainEvent>
{
    public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("Project created, publishing to outbox");
        await outbox.WriteAsync(new ProjectCreated
        {
            OrganisationId = notification.OrganisationId,
            ProjectId = notification.ProjectId,
            ProjectName = notification.Name
        }, cancellationToken);
    }
}