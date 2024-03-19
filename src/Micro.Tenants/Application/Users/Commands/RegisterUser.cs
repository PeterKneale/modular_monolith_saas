using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Tenants.Application.Queue;
using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users.Commands;

public static class RegisterUser
{
    public record Command(Guid UserId, string FirstName, string LastName, string Email, string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IUserRepository users, IQueueRepository queue) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var userId = new UserId(command.UserId);
            var userEmail = new EmailAddress(command.Email);
            var userName = new UserName(command.FirstName, command.LastName);
            var userPassword = new Password(command.Password);
            var userCredentials = new UserCredentials(userEmail, userPassword);

            if (await users.GetAsync(userId, token) != null) throw new AlreadyExistsException(nameof(User), userId);

            if (await users.GetAsync(userEmail, token) != null) AlreadyExistsException.ThrowBecauseEmailAlreadyExists(nameof(User), userEmail);

            var user = User.CreateInstance(userId, userName, userCredentials);
            await users.CreateAsync(user, token);

            var sendEmail = new SendWelcomeEmail.Command { UserId = userId };
            await queue.CreateAsync(QueueMessage.CreateFrom(sendEmail), token);
        }
    }
}