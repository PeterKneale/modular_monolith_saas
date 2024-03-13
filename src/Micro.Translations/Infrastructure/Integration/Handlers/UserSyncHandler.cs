using Micro.Tenants.IntegrationEvents;
using Micro.Translations.Domain.UserAggregate;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Infrastructure.Integration.Handlers;

public class UserSyncHandler(Db db, ILogger<UserSyncHandler> logs) : INotificationHandler<UserCreated>, INotificationHandler<UserChanged>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Syncing user created: {notification.Name}");
        await db.Users.AddAsync(new User
        {
            Id = notification.UserId,
            Name = notification.Name
        }, cancellationToken);
    }

    public async Task Handle(UserChanged notification, CancellationToken cancellationToken)
    {
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == notification.UserId, cancellationToken);
        if (user == null)
        {
            logs.LogInformation($"Syncing user changed (inserting): {notification.UserId}");
            await db.Users.AddAsync(new User
            {
                Id = notification.UserId,
                Name = notification.Name
            }, cancellationToken);
        }
        else
        {
            logs.LogInformation($"Syncing user changed (updating): {notification.UserId}");
            user.Name = notification.Name;
            db.Users.Update(user);
        }
    }
}