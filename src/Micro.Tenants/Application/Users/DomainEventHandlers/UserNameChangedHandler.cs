using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.Users.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Users.DomainEventHandlers;

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