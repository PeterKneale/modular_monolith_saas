using Micro.Translations.Application;
using Micro.Translations.Domain.Languages;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Repositories;

internal class LanguageRepository(ConnectionFactory connections) : ILanguageRepository
{
    public async Task CreateAsync(Language language, CancellationToken token)
    {
        const string sql = $"INSERT INTO {LanguagesTable} ({IdColumn},{ProjectIdColumn},{CodeColumn}) VALUES (@Id, @ProjectId, @Code)";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new
        {
            language.Id,
            language.ProjectId,
            language.LanguageCode.Code
        }, cancellationToken: token));
    }
}