namespace Micro.Tenants.Infrastructure.Infrastructure.Integration;

public class QueueJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessQueueCommand());
}