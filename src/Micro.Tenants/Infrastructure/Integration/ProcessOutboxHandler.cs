using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Infrastructure.Database;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessOutboxCommandHandler(Db db, OutboxMessagePublisher publisher) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await QueryHelper.GetMessagesToPublish(db.Outbox, cancellationToken);

        foreach (var message in messages)
        {
            await publisher.PublishToBus(message, cancellationToken);
            db.Outbox.Remove(message);
        }

        return Unit.Value;
    }
}