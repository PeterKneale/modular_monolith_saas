using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users.Commands;

public static class UpdateUserName
{
    public record Command(string FirstName, string LastName) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IExecutionContext context, IUserRepository users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var userId = context.UserId;
            var name = new UserName(command.FirstName, command.LastName);

            var user = await users.GetAsync(userId, token);
            if (user == null) throw new NotFoundException(nameof(User), userId.Value);
            user.ChangeName(name);
            users.Update(user);

            
        }
    }
}