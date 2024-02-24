using Micro.Common.Application;

namespace Micro.Translations.Application.Translations.Queries;

public static class CountProjectTranslations
{
    public record Query : IRequest<int>;

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            const string sql = $"SELECT COUNT(1) FROM {Constants.TranslationsTable} " +
                               $"JOIN {Constants.TermsTable} on {Constants.TranslationsTable}.{Constants.TermIdColumn} = {Constants.TermsTable}.{Constants.IdColumn} " +
                               $"WHERE {Constants.ProjectIdColumn} = @ProjectId ";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                projectId
            }, cancellationToken: token));
        }
    }
}