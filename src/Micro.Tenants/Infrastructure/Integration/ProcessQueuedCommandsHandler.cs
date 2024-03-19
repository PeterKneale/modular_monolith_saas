using Micro.Common.Infrastructure.Integration.Queue;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessQueuedCommandsHandler(IQueueRepository inbox, ILogger<ProcessQueuedCommandsHandler> log) : IRequestHandler<ProcessQueuedCommand>
{
    public async Task Handle(ProcessQueuedCommand queuedCommand, CancellationToken cancellationToken)
    {
        var messages = await inbox.ListPending(cancellationToken);
        log.LogInformation($"Found {messages.Count} pending commands in queue.");
        foreach (var message in messages)
        {
            await CommandExecutor.SendCommand(QueueMessage.ToRequest(message));
            message.MarkProcessed();
            inbox.Update(message);
        }
    }
}