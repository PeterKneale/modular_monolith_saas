using Micro.Tenants.IntegrationEvents;
using Micro.Translations.Domain.UserAggregate;

namespace Micro.Translations.Infrastructure.Integration.Handlers;

public class UserCreatedHandler(Db db, ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreated>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Syncing user created: {notification.Name}");
        await db.Users.AddAsync(new User
        {
            Id = notification.UserId,
            Name = notification.Name
        }, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}