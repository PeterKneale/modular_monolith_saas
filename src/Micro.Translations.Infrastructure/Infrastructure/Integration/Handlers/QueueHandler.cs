﻿using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Queue;

namespace Micro.Translations.Infrastructure.Infrastructure.Integration.Handlers;

public class QueueHandler(IDbSetQueue set, ILogger<QueueHandler> log) : IRequestHandler<ProcessQueueCommand>
{
    public async Task Handle(ProcessQueueCommand command, CancellationToken cancellationToken)
    {
        var messages = await set.Queue
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        log.LogDebug($"Found {messages.Count} pending commands in queue.");

        foreach (var message in messages)
        {
            await CommandExecutor.SendCommand(QueueMessage.ToRequest(message));
            message.MarkProcessed();
            set.Queue.Update(message);
        }
    }
}