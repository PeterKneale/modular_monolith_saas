using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Translations.Infrastructure.Database.Repositories;

internal class OutboxRepository(Db db) : IOutboxRepository
{
    public async Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token)
    {
        var message = OutboxMessage.CreateFrom(integrationEvent);
        await db.Outbox.AddAsync(message, token);
    }
}