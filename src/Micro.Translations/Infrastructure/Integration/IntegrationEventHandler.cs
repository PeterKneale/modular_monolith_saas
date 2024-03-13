using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    public async Task Handle(IntegrationEvent @event)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        db.Inbox.Add(new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = @event.GetType().AssemblyQualifiedName!,
            Data = JsonConvert.SerializeObject(@event)
        });
        await db.SaveChangesAsync();
    }
}