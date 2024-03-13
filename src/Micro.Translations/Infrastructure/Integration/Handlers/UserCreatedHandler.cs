using Micro.Tenants.IntegrationEvents;

namespace Micro.Translations.Infrastructure.Integration.Handlers;

public class UserCreatedHandler(ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreated>
{
    public Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"User created: {notification.UserId}");
        return Task.CompletedTask;
    }
}