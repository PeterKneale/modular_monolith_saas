namespace Micro.Users.Application.Users.Commands;

public static class ForgotPassword
{
    public record Command(string email) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
    }

    public class Handler(IUserRepository users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var email = EmailAddress.Create(command.email);

            var user = await users.GetAsync(email, token);
            if (user == null) throw new NotFoundException(nameof(User), email);

            user.ForgotPassword();

            users.Update(user);
        }
    }
}