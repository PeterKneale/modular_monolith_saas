namespace Micro.Translations.Application.Queries;

public static class CountTranslations
{
    public record Query : IRequest<int>;

    private class Handler(IDbConnection db, IExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var project = context.ProjectId;
            var sql = """
                      select count(t.id)
                      from translate.translations t
                          join translate.terms term on t.term_id = term.id
                      where term.project_id = @project
                      """;
            var command = new CommandDefinition(sql, new { project }, cancellationToken: token);
            return await db.ExecuteScalarAsync<int>(command);
        }
    }
}