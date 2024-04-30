using Micro.Tenants.Infrastructure.Integration.Handlers;

namespace Micro.Tenants.Infrastructure.Integration;

public class OutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessOutboxCommand());
}