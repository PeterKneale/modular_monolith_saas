namespace Micro.Users.Application.Users.Queries;

public static class GetUserId
{
    public record Query(string Email) : IRequest<Guid>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class Handler(IUserRepository users) : IRequestHandler<Query, Guid>
    {
        public async Task<Guid> Handle(Query query, CancellationToken token)
        {
            var email = EmailAddress.Create(query.Email);
            var user = await users.GetAsync(email, token);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), email);
            }

            return user.Id;
        }
    }
}