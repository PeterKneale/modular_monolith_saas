using Micro.Translations.Domain.Translations;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Translations.Queries;

public static class GetTranslation
{
    public record Query(Guid TranslationId) : IRequest<Result>;

    public record Result(string Text);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.TranslationId).NotEmpty();
        }
    }

    private class Handler(Db db) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var translationId = new TranslationId(query.TranslationId);

            var translation = await db.Translations
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == translationId, token);

            if (translation == null) throw new NotFoundException(translationId);

            return new Result(translation.Text.Value);
        }
    }
}