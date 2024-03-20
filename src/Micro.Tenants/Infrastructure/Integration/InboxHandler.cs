using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration;

public class InboxHandler(Db db, IPublisher publisher, ILogger<InboxHandlerBase> log) : InboxHandlerBase(db, publisher, log), IRequestHandler<ProcessInboxCommand>
{
}