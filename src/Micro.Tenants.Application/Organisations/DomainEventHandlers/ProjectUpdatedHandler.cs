using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.Messages;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class ProjectUpdatedHandler(OutboxWriter outbox, ILogger<ProjectUpdatedHandler> logs) : INotificationHandler<ProjectUpdatedDomainEvent>
{
    public async Task Handle(ProjectUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("Project updated, publishing to outbox");
        await outbox.WriteAsync(new ProjectUpdated
        {
            ProjectId = notification.ProjectId,
            ProjectName = notification.Name
        }, cancellationToken);
    }
}