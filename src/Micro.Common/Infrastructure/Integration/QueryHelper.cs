using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration;

public static class QueryHelper
{
    public static async Task<List<OutboxMessage>> GetMessagesToPublish(DbSet<OutboxMessage> messages, CancellationToken token)
    {
        return await messages
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(token);
    }
    
    public static async Task<List<InboxMessage>> GetMessagesToPublish(DbSet<InboxMessage> messages, CancellationToken token)
    {
        return await messages
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(token);
    }
}