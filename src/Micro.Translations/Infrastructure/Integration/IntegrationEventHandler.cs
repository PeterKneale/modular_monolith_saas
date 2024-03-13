using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Integration;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IntegrationEvent @event)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        db.Inbox.Add(InboxMessage.CreateFrom(@event));
        await db.SaveChangesAsync();
    }
}