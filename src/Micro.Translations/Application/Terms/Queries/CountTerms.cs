using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Terms.Queries;

public static class CountTerms
{
    public record Query : IRequest<int>;

    private class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;

            return await db.Terms
                .AsNoTracking()
                .CountAsync(x => x.ProjectId == projectId, token);
        }
    }
}