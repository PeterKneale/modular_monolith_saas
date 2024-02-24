using System.Data;
using Micro.Common.Infrastructure.Database;
using static Micro.Translations.Constants;

namespace Micro.Translations.Application.Translations;

public static class GetTranslation
{
    public record Query(Guid TranslationId) : IRequest<Result>;

    public record Result(string Text);

    private class Handler(ConnectionFactory connections) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT text FROM {TranslationsTable} WHERE id = @Id";
            using var con = connections.CreateConnection();
            var text = await con.ExecuteScalarAsync<string>(new CommandDefinition(sql, new { Id = query.TranslationId }, cancellationToken: token));
            return new Result(text!);
        }
    }
}