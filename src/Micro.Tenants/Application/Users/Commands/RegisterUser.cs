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

    public class Handler(IUserRepository users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var userId = new UserId(command.UserId);
            if (await users.GetAsync(userId, token) != null) throw new Exception("User already exists");

            var userName = new UserName(command.FirstName, command.LastName);
            var userEmail = new EmailAddress(command.Email);
            var userPassword = new Password(command.Password);
            var userCredentials = new UserCredentials(userEmail, userPassword);
            var user = User.CreateInstance(userId, userName, userCredentials);
            await users.CreateAsync(user, token);
        }
    }
}