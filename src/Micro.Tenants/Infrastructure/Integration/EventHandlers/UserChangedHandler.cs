using Micro.Tenants.Domain.Users;
using Micro.Users.IntegrationEvents;

namespace Micro.Tenants.Infrastructure.Integration.EventHandlers;

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
                Id = new UserId(notification.UserId),
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