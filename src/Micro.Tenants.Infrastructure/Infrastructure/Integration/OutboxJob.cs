namespace Micro.Tenants.Infrastructure.Infrastructure.Integration;

public class OutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessOutboxCommand());
}