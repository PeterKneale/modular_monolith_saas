using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessOutboxHandler(IOutboxRepository outbox, OutboxMessagePublisher publisher) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        foreach (var message in await outbox.ListPending(cancellationToken))
        {
            await publisher.PublishToBus(message, cancellationToken);
            message.MarkProcessed();
            outbox.Update(message);
        }
    }
}