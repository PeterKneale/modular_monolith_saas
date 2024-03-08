using Micro.Translations.Domain.TermAggregate;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Queries;

public static class CountLanguageTranslations
{
    public record Query(string LanguageCode) : IRequest<int>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageCode).NotEmpty();
        }
    }

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var language = Language.FromIsoCode(query.LanguageCode);

            return await db.Translations
                .Where(x => x.LanguageCode == language && x.Term.ProjectId == projectId)
                .AsNoTracking()
                .CountAsync(token);
        }
    }
}