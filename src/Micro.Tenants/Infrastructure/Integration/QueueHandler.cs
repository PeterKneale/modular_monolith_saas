using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration;

public class QueueHandler(Db db, ILogger<QueueHandler> log) : QueueHandlerBase(db, CommandExecutor.SendCommand, log), IRequestHandler<ProcessQueuedCommand>;