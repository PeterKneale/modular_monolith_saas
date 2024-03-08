using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Queries;

public static class ListLanguages
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(string Code, string Name);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var languages = await db.Translations
                .Where(x => x.Term.ProjectId == projectId)
                .Select(x => x.Language)
                .Distinct()
                .AsNoTracking()
                .ToListAsync(token);

            return languages.Select(x => new Result(x.Code, x.Name));
        }
    }
}