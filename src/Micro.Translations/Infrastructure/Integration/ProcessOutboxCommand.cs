using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessOutboxCommand(Db db, OutboxMessagePublisher publisher) : IRequestHandler<Common.Application.ProcessOutboxCommand>
{
    public async Task Handle(Common.Application.ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await QueryHelper.GetMessagesToPublish(db.Outbox, cancellationToken);

        foreach (var message in messages)
        {
            await publisher.PublishToBus(message, cancellationToken);
            db.Remove(message);
        }

        
    }
}