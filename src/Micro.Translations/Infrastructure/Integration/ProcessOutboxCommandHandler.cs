using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessOutboxCommandHandler(Db db, OutboxMessagePublisher publisher, ILogger<ProcessOutboxCommandHandler> logs) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await QueryHelper.GetMessagesToPublish(db.Outbox, cancellationToken);

        foreach (var message in messages)
        {
            await publisher.PublishToBus(message, cancellationToken);
            db.Remove(message);
        }

        return Unit.Value;
    }
}