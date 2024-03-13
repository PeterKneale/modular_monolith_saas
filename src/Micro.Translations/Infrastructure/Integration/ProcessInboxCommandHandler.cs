using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessInboxCommandHandler(Db db, IPublisher publisher, ILogger<ProcessInboxCommandHandler> log) : IRequestHandler<ProcessInboxCommand>
{
    public async Task<Unit> Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await db.Inbox
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            await publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken);
        }

        return Unit.Value;
    }
}