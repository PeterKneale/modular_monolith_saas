using Micro.Translations.Domain.Languages;
using Micro.Translations.Domain.Terms;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Translations.Queries;

public static class GetTranslation
{
    public record Query(Guid TermId, string LanguageCode) : IRequest<Result>;

    public record Result(string Text);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.TermId).NotEmpty();
            RuleFor(m => m.LanguageCode).NotEmpty();
        }
    }

    private class Handler(Db db) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var termId = new TermId(query.TermId);
            var language = Language.FromIsoCode(query.LanguageCode);

            var term = await db.Terms
                .AsNoTracking()
                .Include(x => x.Translations)
                .SingleOrDefaultAsync(x => x.Id == termId, token);

            if (term == null) throw new NotFoundException(termId);
            var translation = term.GetTranslation(language);

            return new Result(translation.Text.Value);
        }
    }
}