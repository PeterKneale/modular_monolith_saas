using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.ApiKeys.Queries;

public static class GetUserApiKeyById
{
    public record Query(Guid Id) : IRequest<string>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class Handler(IApiKeyRepository keys) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken token)
        {
            var id = new UserApiKeyId(query.Id);

            var item = await keys.GetById(id, token);
            if (item == null) throw new NotFoundException(nameof(UserApiKey), id.Value);

            return item.ApiKey.Key.Value;
        }
    }
}