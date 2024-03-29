namespace Micro.Users.Application.Users.Queries;

public static class GetEmail
{
    public record Query(Guid UserId) : IRequest<string>;

    public class Handler(IUserRepository users) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken token)
        {
            var userId = new UserId(query.UserId);
            var user = await users.GetAsync(userId, token);
            if (user == null) throw new BusinessRuleBrokenException("User not found.");
            return user.Credentials.Email;
        }
    }
}