namespace Micro.Users.Application.Users.Commands;

public static class VerifyUser
{
    public record Command(Guid UserId, string Verification) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.Verification).NotEmpty();
        }
    }

    public class Handler(IUserRepository users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = new UserId(command.UserId);
            var verification = command.Verification;

            var user = await users.GetAsync(userId, cancellationToken);
            if (user == null) throw new NotFoundException(nameof(User), userId.Value);

            user.Verify(verification);
        }
    }
}