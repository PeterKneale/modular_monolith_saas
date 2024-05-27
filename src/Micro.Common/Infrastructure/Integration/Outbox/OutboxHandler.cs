using Micro.Common.Application;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration.Outbox;

public class OutboxHandler(IDbSetOutbox set, OutboxMessagePublisher publisher, ILogger<OutboxHandler> log) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await set.Outbox
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        log.LogTrace($"Found {messages.Count} pending messages in outbox.");

        foreach (var message in messages)
        {
            await publisher.PublishToBus(message, cancellationToken);
            message.MarkProcessed();
            set.Outbox.Update(message);
        }
    }
}