namespace Micro.Translations.Application.Queries;

public static class GetLanguage
{
    public record Query(string LanguageCode) : IRequest<Result>;

    public record Result(Guid Id, string Name, string Code);

    public class Validator : AbstractValidator<Query>;

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageCode = query.LanguageCode;

            var sql = """
                      SELECT id, name, code
                      FROM languages
                      WHERE project_id = @projectId AND code = @languageCode
                      """;

            var command = new CommandDefinition(sql, new { projectId, languageCode }, cancellationToken: token);
            return await db.QuerySingleAsync<Result>(command);
        }
    }
}