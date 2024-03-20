using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Integration;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IIntegrationEvent integrationEvent, CancellationToken token)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var inbox = scope.ServiceProvider.GetRequiredService<IInboxRepository>();
        await inbox.CreateAsync(integrationEvent, token);
        await db.SaveChangesAsync(token);
    }
}