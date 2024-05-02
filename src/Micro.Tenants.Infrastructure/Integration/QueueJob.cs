using Micro.Tenants.Infrastructure.Integration.Handlers;

namespace Micro.Tenants.Infrastructure.Integration;

[ExcludeFromCodeCoverage]
public class QueueJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessQueueCommand());
}