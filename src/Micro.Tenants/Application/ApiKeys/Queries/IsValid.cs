using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys.Queries;

public static class IsValid
{
    public record Query(string ApiKeyValue) : IRequest<Result>;

    public record Result(bool Valid, Guid? UserId = null);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.ApiKeyValue).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IApiKeyRepository keys) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var value = new ApiKeyValue(query.ApiKeyValue);

            var userApiKey = await keys.GetAsync(value, token);
            
            return userApiKey != null 
                ? new Result(true, userApiKey.UserId.Value) 
                : new Result(false);
        }
    }
}