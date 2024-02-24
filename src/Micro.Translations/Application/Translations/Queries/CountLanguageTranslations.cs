using Micro.Common.Application;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations.Queries;

public static class CountLanguageTranslations
{
    public record Query(Guid LanguageId) : IRequest<int>;

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query query, CancellationToken token)
        {
            var projectId = context.ProjectId;
            var languageId = query.LanguageId;
            const string sql = $"SELECT COUNT(1) FROM {TranslationsTable} " +
                               $"JOIN {TermsTable} on {TranslationsTable}.{TermIdColumn} = {TermsTable}.{IdColumn} " +
                               $"WHERE {ProjectIdColumn} = @ProjectId " +
                               $"AND {TranslationsTable}.{LanguageIdColumn} = @LanguageId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                ProjectId = projectId,
                LanguageId = languageId
            }, cancellationToken: token));
        }
    }
}