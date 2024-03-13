using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessInboxCommandHandler(Db db, IPublisher publisher, ILogger<ProcessInboxCommandHandler> logs) : IRequestHandler<ProcessInboxCommand>
{
    public async Task<Unit> Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await QueryHelper.GetMessagesToPublish(db.Inbox, cancellationToken);

        foreach (var message in messages)
        {
            await publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken);
            db.Remove(message);
        }

        return Unit.Value;
    }
}