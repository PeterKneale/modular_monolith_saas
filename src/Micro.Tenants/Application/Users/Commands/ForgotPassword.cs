using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users.Commands;

public static class ForgotPassword
{
    public record Command(string email) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
    }

    public class Handler(IUserRepository users) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command command, CancellationToken token)
        {
            var email = new EmailAddress(command.email);

            var user = await users.GetAsync(email, token);
            if (user == null) throw new NotFoundException(nameof(User), email);

            // todo: generate reset token
            // todo: send email with reset link

            users.Update(user);

            return Unit.Value;
        }
    }
}