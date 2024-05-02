using Micro.Tenants.Infrastructure.Integration.Handlers;

namespace Micro.Tenants.Infrastructure.Integration;

[ExcludeFromCodeCoverage]
public class InboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessInboxCommand());
}