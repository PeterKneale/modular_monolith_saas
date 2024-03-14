using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessInboxCommand(Db db, IPublisher publisher) : IRequestHandler<Common.Application.ProcessInboxCommand>
{
    public async Task<Unit> Handle(Common.Application.ProcessInboxCommand command, CancellationToken cancellationToken)
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