namespace Micro.Users.Application.ApiKeys.Queries;

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