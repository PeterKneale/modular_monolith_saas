using Micro.Users.Infrastructure.Integration.Handlers;
using Quartz;

namespace Micro.Users.Infrastructure.Integration;

public class QueueJob : IJob
{
    public async Task Execute(IJobExecutionContext context) => 
        await CommandExecutor.SendCommand(new ProcessQueueCommand());
}