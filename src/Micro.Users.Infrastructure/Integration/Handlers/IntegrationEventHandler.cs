﻿using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Users.Infrastructure.Database;

namespace Micro.Users.Infrastructure.Integration.Handlers;

[ExcludeFromCodeCoverage]
public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IIntegrationEvent integrationEvent, CancellationToken token)
    {
        using var scope = UsersCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var inbox = scope.ServiceProvider.GetRequiredService<InboxWriter>();
        await inbox.WriteAsync(integrationEvent, token);
        await db.SaveChangesAsync(token);
    }
}