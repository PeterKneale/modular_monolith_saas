using Micro.Common.Application;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration.Inbox;

public class InboxHandlerBase(IInboxDbSet set, IPublisher publisher, ILogger<InboxHandlerBase> log)
{
    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await set.Inbox
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        log.LogInformation($"Found {messages.Count} pending messages in inbox.");
        foreach (var message in messages)
        {
            await publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken);
            message.MarkProcessed();
            set.Inbox.Update(message);
        }
    }
}