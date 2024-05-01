using System.Globalization;

namespace Micro.Translations.Application.Queries;

public static class ListLanguages
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(string Name, string Code);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var sql = """
                      SELECT language_code AS Code
                      FROM translate.languages
                      WHERE project_id = @projectId
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            var codes =await db.QueryAsync<string>(command);
            return codes.Select(x => new Result(CultureInfo.GetCultureInfo(x).DisplayName, x));
        }
    }
}