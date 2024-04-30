using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration.Handlers;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IIntegrationEvent integrationEvent, CancellationToken token)
    {
        using var scope = TenantsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var inbox = scope.ServiceProvider.GetRequiredService<InboxWriter>();
        await inbox.WriteAsync(integrationEvent, token);
        await db.SaveChangesAsync(token);
    }
}