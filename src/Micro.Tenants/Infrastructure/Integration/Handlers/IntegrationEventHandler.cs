using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration.Handlers;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IIntegrationEvent integrationEvent, CancellationToken token)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var inbox = scope.ServiceProvider.GetRequiredService<InboxWriter>();
        await inbox.WriteAsync(integrationEvent, token);
        await db.SaveChangesAsync(token);
    }
}