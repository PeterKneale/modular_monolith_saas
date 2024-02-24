using Micro.Common.Application;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Terms.Queries;

public static class ListTerms
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name);

    private class Handler(ConnectionFactory connections, IProjectExecutionContext context) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT {IdColumn}, {NameColumn} FROM {TermsTable} WHERE {ProjectIdColumn} = @ProjectId";
            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
            {
                context.ProjectId
            }, cancellationToken: token));
            return Map(reader);
        }

        private static IEnumerable<Result> Map(IDataReader reader)
        {
            var result = new List<Result>();
            while (reader.Read())
            {
                var id = reader.GetGuid(0);
                var name = reader.GetString(1);
                result.Add(new Result(id,name));
            }
            return result;
        }
    }
}