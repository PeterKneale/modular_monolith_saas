using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Translations.Queries;

public static class CountProjectTranslations
{
    public record Query : IRequest<int>;

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;

            return await db.Translations
                .Where(x => x.Term.ProjectId == projectId)
                .AsNoTracking()
                .CountAsync(cancellationToken: token);
        }
    }
}