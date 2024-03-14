using Micro.Tenants.Domain.Users;

namespace Micro.Tenants.Application.Users.Queries;

public static class CanAuthenticate
{
    public record Query(string Email, string Password) : IRequest<Result>;

    public record Result(bool Success, Guid? UserId = null);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IUserRepository users) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var email = new EmailAddress(query.Email);
            var password = new Password(query.Password);
            var credentials = new UserCredentials(email, password);

            var user = await users.GetAsync(credentials.Email, token);
            if (user == null) return new Result(false);

            var success = user.CanLogin(credentials);
            return success
                ? new Result(true, user.Id.Value)
                : new Result(false);
        }
    }
}