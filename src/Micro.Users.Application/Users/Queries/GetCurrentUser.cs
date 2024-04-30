namespace Micro.Users.Application.Users.Queries;

public static class GetCurrentUser
{
    public record Query : IRequest<Guid>;

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IUserRepository users, IExecutionContext context) : IRequestHandler<Query, Guid>
    {
        public async Task<Guid> Handle(Query query, CancellationToken token)
        {
            var userId = context.UserId;
            var user = await users.GetAsync(userId, token);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId.Value);
            }

            return user.Id;
        }
    }
}