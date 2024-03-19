using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class OutboxRepository(Db db) : IOutboxRepository
{
    public async Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token) =>
        await db.Outbox.AddAsync(OutboxMessage.CreateFrom(integrationEvent), token);

    public void Update(OutboxMessage message) =>
        db.Outbox.Update(message);

    public async Task<List<OutboxMessage>> ListPending(CancellationToken token) =>
        await db.Outbox
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(token);
}