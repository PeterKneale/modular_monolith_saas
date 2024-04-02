using Micro.Tenants.Domain.UserAggregate;
using Micro.Users.IntegrationEvents;

namespace Micro.Tenants.Infrastructure.Integration.EventHandlers;

internal class UserCreatedHandler(Db db, ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreated>
{
    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Syncing user created: {notification.Name}");
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