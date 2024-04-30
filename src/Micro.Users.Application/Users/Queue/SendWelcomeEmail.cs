using Micro.Common.Infrastructure.Integration;

namespace Micro.Users.Application.Users.Queue;

public static class SendWelcomeEmail
{
    public class Command : IQueuedCommand
    {
        public UserId UserId { get; init; } = null!;
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public class Handler(IUserRepository users, ILogger<Handler> logs) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await users.GetAsync(userId, cancellationToken);
            if (user == null) throw new NotFoundException(nameof(User), userId.ToString());

            var actionUrl = $"http://localhost:8080/Auth/VerifyEmail?userId={user.Id}&token={user.VerificationToken}";
            
            logs.LogInformation($"Sending welcome email to {user.EmailAddress} with action url {actionUrl}");
        }
    }
}