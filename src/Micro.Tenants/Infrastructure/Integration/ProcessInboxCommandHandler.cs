using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessInboxCommandHandler(Db db, IPublisher publisher) : IRequestHandler<ProcessInboxCommand>
{
    public async Task<Unit> Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await QueryHelper.GetMessagesToPublish(db.Inbox, cancellationToken);

        foreach (var message in messages)
        {
            await publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken);
            db.Inbox.Remove(message);
        }

        return Unit.Value;
    }
}