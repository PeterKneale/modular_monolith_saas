using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Users.Domain.Users.DomainEvents;
using Micro.Users.IntegrationEvents;

namespace Micro.Users.Application.Users.DomainEventHandlers;

public class UserNameChangedHandler(OutboxWriter outbox, ILogger<UserCreatedHandler> logs) : INotificationHandler<UserNameChangedDomainEvent>
{
    public async Task Handle(UserNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("User changed, publishing to outbox");
        await outbox.WriteAsync(new UserChanged
        {
            UserId = notification.Id,
            Name = notification.Name
        }, cancellationToken);
    }
}