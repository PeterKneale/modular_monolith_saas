using Micro.Translations.Infrastructure.Database;

namespace Micro.Translations.Application.Languages.Queries;

public static class ListLanguages
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name, string Code);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(Db db, IProjectExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;

            var languages = await db.Languages
                .Where(x => x.ProjectId == projectId)
                .AsNoTracking()
                .ToListAsync(token);

            return languages.Select(x => new Result(x.Id.Value, x.LanguageCode.Name, x.LanguageCode.Code));
        }
    }
}