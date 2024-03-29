namespace Micro.Common.Infrastructure.Integration.Queue;

public class QueueWriter(IDbSetQueue set)
{
    public async Task WriteAsync(IQueuedCommand command, CancellationToken token) =>
        await set.Queue.AddAsync(QueueMessage.CreateFrom(command), token);
}