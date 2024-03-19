using Quartz;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessQueuedCommandsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.SendCommand(new ProcessQueuedCommand());
    }
}