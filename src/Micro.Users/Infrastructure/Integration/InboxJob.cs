using Micro.Users.Infrastructure.Integration.Handlers;
using Quartz;

namespace Micro.Users.Infrastructure.Integration;

public class InboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) => 
        await CommandExecutor.SendCommand(new ProcessInboxCommand());
}