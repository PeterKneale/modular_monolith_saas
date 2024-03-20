using Micro.Common.Application;
using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration.Queue;

public class QueueHandlerBase(IQueueDbSet queue, Func<IRequest, Task> executor, ILogger<QueueHandlerBase> log)
{
    public async Task Handle(ProcessQueuedCommand queuedCommand, CancellationToken cancellationToken)
    {
        var messages = await queue.Commands
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        log.LogInformation($"Found {messages.Count} pending commands in queue.");

        foreach (var message in messages)
        {
            await executor(QueueMessage.ToRequest(message));
            message.MarkProcessed();
            queue.Commands.Update(message);
        }
    }
}