using System.Globalization;

namespace Micro.Translations.Application.Queries;

public static class ListLanguages
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name, string Code);

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var sql = """
                      SELECT id, name, code
                      FROM languages
                      WHERE project_id = @projectId
                      """;
            var command = new CommandDefinition(sql, new { projectId }, cancellationToken: token);
            return await db.QueryAsync<Result>(command);
        }
    }
}