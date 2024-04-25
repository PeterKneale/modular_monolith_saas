namespace Micro.Users.Application.ApiKeys.Queries;

public static class CanAuthenticate
{
    public record Query(string ApiKeyValue) : IRequest<Result>;

    public record Result(bool Valid, Guid? UserId = null, string? Email = null);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.ApiKeyValue).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IApiKeyRepository keys, IUserRepository users) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var value = new ApiKeyValue(query.ApiKeyValue);

            var userApiKey = await keys.GetByKey(value, token);
            if (userApiKey == null)
            {
                return new Result(false);
            }

            var userId = userApiKey.UserId;
            
            var user = await users.GetAsync(userId, token);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId.Value);
            }

            var email = user.EmailAddress.Value;

            return new Result(true, userId, email);
        }
    }
}