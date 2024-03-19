using Micro.Common.Infrastructure.Integration.Outbox;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessOutboxHandler(IOutboxRepository outbox, OutboxMessagePublisher publisher, ILogger<ProcessOutboxHandler> log) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await outbox.ListPending(cancellationToken);
        log.LogInformation($"Found {messages.Count} pending messages in outbox.");
        foreach (var message in messages)
        {
            await publisher.PublishToBus(message, cancellationToken);
            message.MarkProcessed();
            outbox.Update(message);
        }
    }
}