﻿using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.OrganisationAggregate.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Organisations.DomainEventHandlers;

public class OrganisationCreatedHandler(OutboxWriter outbox, ILogger<OrganisationCreatedDomainEvent> logs) : INotificationHandler<OrganisationCreatedDomainEvent>
{
    public async Task Handle(OrganisationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("Organisation created, publishing to outbox");
        await outbox.WriteAsync(new OrganisationCreated
        {
            OrganisationId = notification.Id,
            OrganisationName = notification.Name
        }, cancellationToken);
    }
}