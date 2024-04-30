using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration.Handlers;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IIntegrationEvent integrationEvent, CancellationToken token)
    {
        using var scope = TranslationsCompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        var inbox = scope.ServiceProvider.GetRequiredService<InboxWriter>();
        await inbox.WriteAsync(integrationEvent, token);
        await db.SaveChangesAsync(token);
    }
}