namespace Micro.Users.Application.Users.Queries;

public static class GetResetPasswordToken
{
    public record Query(Guid UserId) : IRequest<string>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
    
    public class Handler(IUserRepository users) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken token)
        {
            var userId = new UserId(query.UserId);
            var user = await users.GetAsync(userId, token);
            if (user == null) throw new BusinessRuleBrokenException("User not found.");
            if (user.ForgotPasswordToken == null) throw new BusinessRuleBrokenException("User has not forgotten their password.");
            return user.ForgotPasswordToken;
        }
    }
}