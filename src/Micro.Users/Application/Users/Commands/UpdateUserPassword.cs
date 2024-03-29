namespace Micro.Users.Application.Users.Commands;

public static class UpdateUserPassword
{
    public record Command(string OldPassword, string NewPassword) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OldPassword).NotEmpty().MaximumLength(50);
            RuleFor(m => m.NewPassword).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IExecutionContext context, IUserRepository users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var userId = context.UserId;

            var user = await users.GetAsync(userId, token);
            if (user == null) throw new NotFoundException(nameof(User), userId.Value);

            var oldPassword = new Password(command.OldPassword);
            var newPassword = new Password(command.NewPassword);

            user.ChangePassword(oldPassword, newPassword);
            users.Update(user);
        }
    }
}