using Micro.Tenants.IntegrationEvents;
using Micro.Translations.Domain.UserAggregate;

namespace Micro.Translations.Infrastructure.Integration.Handlers;

public class UserChangedHandler(Db db, ILogger<UserChangedHandler> logs) : INotificationHandler<UserChanged>
{
    public async Task Handle(UserChanged notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Syncing user changed: {notification.Name}");
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == notification.UserId, cancellationToken);
        if (user == null)
        {
            logs.LogInformation($"Syncing user changed (inserting): {notification.Name}");
            await db.Users.AddAsync(new User
            {
                Id = notification.UserId,
                Name = notification.Name
            }, cancellationToken);
        }
        else
        {
            logs.LogInformation($"Syncing user changed (updating): {notification.Name}");
            user.Name = notification.Name;
            db.Users.Update(user);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}