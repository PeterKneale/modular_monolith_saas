using Micro.Translations.Domain.Languages;
using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Translations.Queries;

public static class CountLanguageTranslations
{
    public record Query(Guid LanguageId) : IRequest<int>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.LanguageId).NotEmpty();
        }
    }

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = new LanguageId(query.LanguageId);

            return await db.Translations
                .Where(x => x.LanguageId == languageId && x.Term.ProjectId == projectId)
                .AsNoTracking()
                .CountAsync(cancellationToken: token);
        }
    }
}