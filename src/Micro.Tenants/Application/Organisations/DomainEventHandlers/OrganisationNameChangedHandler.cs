using Micro.Common.Infrastructure.Outbox;
using Micro.Tenants.Domain.Organisations.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationNameChangedHandler(IOutboxRepository events) : INotificationHandler<OrganisationNameChangedDomainEvent>
{
    public async Task Handle(OrganisationNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var id = notification.Id;
        var name = notification.Name;
        await events.CreateAsync(new OrganisationChanged { OrganisationId = id, OrganisationName = name }, cancellationToken);
    }
}