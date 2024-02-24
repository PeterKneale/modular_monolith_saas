using Micro.Translations.Domain;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Languages.Queries;

public static class ListLanguages
{
    public record Query(Guid ProjectId) : IRequest<IEnumerable<Result>>;

    public record Result(string Name, string Code);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.ProjectId).NotEmpty();
        }
    }

    public class Handler(ConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT {CodeColumn} FROM {LanguagesTable} WHERE {ProjectIdColumn} = @ProjectId;";
            using var con = connections.CreateConnection();
            var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
            {
                query.ProjectId
            }, cancellationToken: token));
            var list = new List<Result>();
            while (reader.Read())
            {
                var code = reader.GetString(0);
                var language = LanguageCode.FromIsoCode(code);
                var result = new Result(language.Name, language.Code);
                list.Add(result);
            }

            return list;
        }
    }
}