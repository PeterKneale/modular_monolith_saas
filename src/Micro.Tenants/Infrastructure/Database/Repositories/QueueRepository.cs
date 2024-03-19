using Micro.Common.Infrastructure.Integration.Queue;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class QueueRepository(Db db) : IQueueRepository
{
    public async Task CreateAsync(QueueMessage command, CancellationToken token) =>
        await db.Commands.AddAsync(command, token);

    public void Update(QueueMessage command) =>
        db.Commands.Update(command);

    public async Task<List<QueueMessage>> ListPending(CancellationToken token) =>
        await db.Commands
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(token);
}