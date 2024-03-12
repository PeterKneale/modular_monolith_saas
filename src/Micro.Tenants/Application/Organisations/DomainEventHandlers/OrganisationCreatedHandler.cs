using Micro.Common.Infrastructure.Outbox;
using Micro.Tenants.Domain.Organisations.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationCreatedHandler(IOutboxRepository events) : INotificationHandler<OrganisationCreatedDomainEvent>
{
    public async Task Handle(OrganisationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var id = notification.Id;
        var name = notification.Name;
        await events.CreateAsync(new OrganisationCreated { OrganisationId = id, OrganisationName = name }, cancellationToken);
    }
}