using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class InboxRepository(Db db) : IInboxRepository
{
    public async Task CreateAsync(IntegrationEvent integrationEvent, CancellationToken token)
    {
        var type = integrationEvent.GetType().Name;
        var data = JsonConvert.SerializeObject(integrationEvent, Formatting.Indented);
        var record = new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Data = data
        };
        await db.Inbox.AddAsync(record, token);
    }
}