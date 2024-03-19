using Quartz;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessCommandsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.SendCommand(new ProcessQueuedCommand());
    }
}