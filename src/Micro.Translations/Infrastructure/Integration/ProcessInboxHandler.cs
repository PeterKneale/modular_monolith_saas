using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessInboxHandler(IInboxRepository inbox, IPublisher publisher, ILogger<ProcessInboxHandler> log) : IRequestHandler<ProcessInboxCommand>
{
    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await inbox.ListPending(cancellationToken);
        log.LogInformation($"Found {messages.Count} pending messages in inbox.");
        foreach (var message in messages)
        {
            await publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken);
            message.MarkProcessed();
            inbox.Update(message);
        }
    }
}