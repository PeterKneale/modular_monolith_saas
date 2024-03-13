using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Domain.Users.DomainEvents;
using Micro.Tenants.IntegrationEvents;

namespace Micro.Tenants.Application.Users.DomainEventHandlers;

public class UserCreatedHandler(IOutboxRepository outbox, ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("User created, publishing to outbox");
        await outbox.CreateAsync(new UserCreated
        {
            UserId = notification.Id,
            UserName = notification.Name
        }, cancellationToken);
    }
}