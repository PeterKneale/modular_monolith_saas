using Micro.Tenants.Infrastructure.Integration.Handlers;
using Quartz;

namespace Micro.Tenants.Infrastructure.Integration;

public class QueueJob : IJob
{
    public async Task Execute(IJobExecutionContext context) => 
        await CommandExecutor.SendCommand(new ProcessQueueCommand());
}