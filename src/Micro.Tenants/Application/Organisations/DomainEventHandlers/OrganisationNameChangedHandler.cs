using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.Organisations.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationNameChangedHandler(IOutboxRepository outbox) : INotificationHandler<OrganisationNameChangedDomainEvent>
{
    public async Task Handle(OrganisationNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await outbox.CreateAsync(new OrganisationChanged
        {
            OrganisationId = notification.Id,
            OrganisationName = notification.Name
        }, cancellationToken);
    }
}