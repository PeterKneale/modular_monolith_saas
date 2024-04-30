using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Application.Users.Commands;

public static class ResetPassword
{
    public record Command(Guid UserId, string Token, string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.Token).NotEmpty();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IUserRepository users, IHashPassword hasher) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = UserId.Create(command.UserId);
            var token = command.Token;
            var password = Password.Create(command.Password);

            var user = await users.GetAsync(userId, cancellationToken);
            if (user == null) throw new NotFoundException(nameof(User), userId.Value);

            user.ResetPassword(token, password, hasher);
        }
    }
}