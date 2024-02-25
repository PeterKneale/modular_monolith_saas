using Micro.Common.Application;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys.Queries;

public static class GetById
{
    public record Query(Guid Id) : IRequest<string>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class Handler(IApiKeyRepository keys, IUserExecutionContext context) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken token)
        {
            var id = new UserApiKeyId(query.Id);
            var userId = context.UserId;
            
            var item = await keys.GetAsync(userId, id, token);
            if (item == null)
            {
                throw new Exception($"api key not found {query.Id}");
            }

            return item.ApiKey.Key.Value;
        }
    }
}