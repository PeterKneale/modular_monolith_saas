namespace Micro.Translations.Application.Queries;

public static class GetLanguage
{
    public record Query(string LanguageCode) : IRequest<Guid>;

    public class Validator : AbstractValidator<Query>
    {
    }

    public class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, Guid>
    {
        public async Task<Guid> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageCode = query.LanguageCode;
            
            var sql = """
                      SELECT id
                      FROM translate.languages
                      WHERE project_id = @projectId AND language_code = @languageCode
                      """;
            
            var command = new CommandDefinition(sql, new { projectId,languageCode }, cancellationToken: token);
            return await db.ExecuteScalarAsync<Guid>(command);
        }
    }
}