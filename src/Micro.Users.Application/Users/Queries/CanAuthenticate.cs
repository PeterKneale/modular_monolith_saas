using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Application.Users.Queries;

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

    public class Handler(IUserRepository users, ICheckPassword checker) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var email = EmailAddress.Create(query.Email);
            var password = Password.Create(query.Password);

            var user = await users.GetAsync(email, token);
            if (user == null) return new Result(false);

            try
            {
                user.Login(email, password, checker);
                return new Result(true, user.Id);
            }
            catch (BusinessRuleBrokenException)
            {
                return new Result(false);
            }
        }
    }
}