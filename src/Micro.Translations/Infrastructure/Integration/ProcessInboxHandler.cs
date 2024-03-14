using Micro.Common.Infrastructure.Integration.Inbox;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessInboxHandler(IInboxRepository inbox, IPublisher publisher) : IRequestHandler<ProcessInboxCommand>
{
    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        foreach (var message in await inbox.ListPending(cancellationToken))
        {
            await publisher.Publish(InboxMessage.ToIntegrationEvent(message), cancellationToken);
            message.MarkProcessed();
            inbox.Update(message);
        }
    }
}