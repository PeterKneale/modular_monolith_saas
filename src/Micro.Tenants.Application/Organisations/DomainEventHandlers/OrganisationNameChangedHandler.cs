using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.Messages;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationNameChangedHandler(OutboxWriter outbox, ILogger<OrganisationNameChangedHandler> logs) : INotificationHandler<OrganisationNameChangedDomainEvent>
{
    public async Task Handle(OrganisationNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("Organisation changed, publishing to outbox");
        await outbox.WriteAsync(new OrganisationUpdated
        {
            OrganisationId = notification.Id,
            OrganisationName = notification.Name
        }, cancellationToken);
    }
}