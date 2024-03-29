using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.Users.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Users.DomainEventHandlers;

public class UserCreatedHandler(OutboxWriter outbox, ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("User created, publishing to outbox");
        var user = notification.User;
        await outbox.WriteAsync(new UserCreated
        {
            UserId = user.Id,
            Name = user.Name
        }, cancellationToken);
    }
}