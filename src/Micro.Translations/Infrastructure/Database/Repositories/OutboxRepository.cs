using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Translations.Infrastructure.Database.Repositories;

internal class OutboxRepository(Db db) : IOutboxRepository
{
    public async Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token)
    {
        var type = integrationEvent.GetType().FullName!;
        var data = JsonConvert.SerializeObject(integrationEvent, Formatting.Indented);
        var record = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Data = data
        };
        await db.Outbox.AddAsync(record, token);
    }
}