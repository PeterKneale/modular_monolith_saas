using Quartz;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessInboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.SendCommand(new ProcessInboxCommand());
    }
}