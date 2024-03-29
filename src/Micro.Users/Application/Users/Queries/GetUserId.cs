namespace Micro.Users.Application.Users.Queries;

public static class GetUserId
{
    public record Query(string Email) : IRequest<Guid>;

    public class Handler(IUserRepository users) : IRequestHandler<Query, Guid>
    {
        public async Task<Guid> Handle(Query query, CancellationToken token)
        {
            var email = new EmailAddress(query.Email);
            var user = await users.GetAsync(email, token);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), email);
            }

            return user.Id;
        }
    }
}