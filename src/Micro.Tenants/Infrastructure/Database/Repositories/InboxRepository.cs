using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class InboxRepository(Db db) : IInboxRepository
{
    public async Task CreateAsync(IIntegrationEvent integrationEvent, CancellationToken token) =>
        await db.Inbox.AddAsync(InboxMessage.CreateFrom(integrationEvent), token);

    public void Update(InboxMessage message) =>
        db.Inbox.Update(message);

    public async Task<List<InboxMessage>> ListPending(CancellationToken token) =>
        await db.Inbox
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(token);
}