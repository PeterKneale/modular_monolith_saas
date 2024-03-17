using Micro.Tenants.Domain.Users.DomainEvents;

namespace Micro.Tenants.Application.Users.DomainEventHandlers;

public class UserCreatedVerificationLinkHandler(ILogger<UserCreatedHandler> logs) : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logs.LogInformation("User verification link");
        var user = notification.User;
        
        logs.LogInformation($"http://localhost:8080/Auth/Verify?userId={user.Id}&token={user.Verification.VerificationToken}");
    }
}