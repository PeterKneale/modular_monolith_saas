using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Database.Repositories;

internal class InboxRepository(Db db) : IInboxRepository
{
    public async Task CreateAsync(InboxMessage message) => await db.Inbox.AddAsync(message);

    public void Update(InboxMessage message) => db.Inbox.Update(message);

    public async Task<List<InboxMessage>> ListPending(CancellationToken token) =>
        await db.Inbox
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(token);
}