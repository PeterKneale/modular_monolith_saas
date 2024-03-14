using Quartz;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessOutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.SendCommand(new ProcessOutboxCommand());
    }
}