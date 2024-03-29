using Micro.Users.Domain.Users.DomainEvents;

namespace Micro.Users.Application.Users.DomainEventHandlers;

public class UserCreatedVerificationLinkHandler(ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("User verification link");
        var user = notification.User;
        
        logs.LogInformation($"http://localhost:8080/Auth/Verify?userId={user.Id}&token={user.Verification.VerificationToken}");
        return Task.CompletedTask;
    }
}