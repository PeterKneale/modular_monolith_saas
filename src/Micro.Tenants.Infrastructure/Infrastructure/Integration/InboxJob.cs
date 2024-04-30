namespace Micro.Tenants.Infrastructure.Infrastructure.Integration;

public class InboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessInboxCommand());
}