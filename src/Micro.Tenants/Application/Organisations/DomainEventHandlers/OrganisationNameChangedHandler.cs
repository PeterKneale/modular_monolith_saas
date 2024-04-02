using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationNameChangedHandler(OutboxWriter outbox, ILogger<OrganisationNameChangedHandler> logs) : INotificationHandler<OrganisationNameChangedDomainEvent>
{
    public async Task Handle(OrganisationNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("Organisation changed, publishing to outbox");
        await outbox.WriteAsync(new OrganisationChanged
        {
            OrganisationId = notification.Id,
            OrganisationName = notification.Name
        }, cancellationToken);
    }
}