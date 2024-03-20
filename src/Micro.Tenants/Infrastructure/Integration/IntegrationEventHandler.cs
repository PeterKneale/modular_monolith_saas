﻿using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IIntegrationEvent @event)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var inbox = scope.ServiceProvider.GetRequiredService<IInboxRepository>();
        var message = InboxMessage.CreateFrom(@event);
        await inbox.CreateAsync(message);
        await db.SaveChangesAsync();
    }
}