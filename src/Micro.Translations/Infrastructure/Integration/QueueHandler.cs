using Micro.Common.Infrastructure.Integration.Queue;

namespace Micro.Translations.Infrastructure.Integration;

public class QueueHandler(Db db, ILogger<QueueHandler> log) : QueueHandlerBase(db, CommandExecutor.SendCommand, log), IRequestHandler<ProcessQueuedCommand>
{
}