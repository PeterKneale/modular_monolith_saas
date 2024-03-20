namespace Micro.Common.Infrastructure.Integration.Queue;

public interface IQueueRepository
{
    Task CreateAsync(IQueuedCommand command, CancellationToken token);
}

public class QueueRepository(IQueueDbSet set) : IQueueRepository
{
    public async Task CreateAsync(IQueuedCommand command, CancellationToken token) =>
        await set.Commands.AddAsync(QueueMessage.CreateFrom(command), token);
}