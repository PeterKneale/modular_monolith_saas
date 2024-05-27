using Micro.Translations.Infrastructure.Integration.Handlers;
using Quartz;

namespace Micro.Translations.Infrastructure.Integration;

[ExcludeFromCodeCoverage]
public class QueueJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessQueueCommand());
}