using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.Organisations.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationCreatedHandler(IOutboxRepository outbox, ILogger<OrganisationCreatedDomainEvent> logs) : INotificationHandler<OrganisationCreatedDomainEvent>
{
    public async Task Handle(OrganisationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("Organisation created, publishing to outbox");
        await outbox.CreateAsync(new OrganisationCreated
        {
            OrganisationId = notification.Id,
            OrganisationName = notification.Name
        }, cancellationToken);
    }
}