namespace Micro.Tenants.Application.ApiKeys.Queries;

public static class ListUserApiKeys
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IApiKeyRepository keys, IExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var userId = context.UserId;

            var items = await keys.ListAsync(userId, token);

            return items.Select(x => new Result(x.Id.Value, x.ApiKey.Name.Value));
        }
    }
}