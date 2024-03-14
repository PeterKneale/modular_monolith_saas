using Quartz;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessOutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.SendCommand(new Common.Application.ProcessOutboxCommand());
    }
}