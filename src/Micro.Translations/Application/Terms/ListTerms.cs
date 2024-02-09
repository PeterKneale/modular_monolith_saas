using System.Data;
using Dapper;
using Micro.Common.Infrastructure.Database;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Terms;

public static class ListTerms
{
    public record Query(Guid AppId) : IRequest<Result>;

    public record TermResult(Guid Id, string Name);

    public record Result(int TotalTerms, IEnumerable<TermResult> Terms);

    private class Handler(ConnectionFactory connections) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var total = await CountTerms(query.AppId, token);
            var terms = await ListTerms(query.AppId, token);
            var models = terms.Select(x => new TermResult(x.Key, x.Value));
            return new Result(total, models);
        }

        private async Task<int> CountTerms(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT COUNT(1) FROM {TermsTable} WHERE app_id = @AppId";
            using var con = connections.CreateConnection();
            return await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { appId }, cancellationToken: token));
        }

        private async Task<IDictionary<Guid, string>> ListTerms(Guid appId, CancellationToken token)
        {
            var sql = $"SELECT t.id, t.name " +
                      $"FROM {TermsTable} t " +
                      $"WHERE t.app_id = @appId";
                      
            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { appId }, cancellationToken: token));
            return ToDictionary(reader);
        }

        private static Dictionary<Guid, string> ToDictionary(IDataReader reader)
        {
            var result = new Dictionary<Guid, string>();
            while (reader.Read())
            {
                var id = reader.GetGuid(0);
                var name = reader.GetString(1);
                result.Add(id, name);
            }
            return result;
        }
    }
}