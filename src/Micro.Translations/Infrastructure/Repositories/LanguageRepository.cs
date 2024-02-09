using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Application;
using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Infrastructure.Repositories;

internal class LanguageRepository(ConnectionFactory connections) : ILanguageRepository
{
    public async Task CreateAsync(Language language)
    {
        const string sql = $"INSERT INTO {Constants.LanguagesTable} (id, app_id, name, code) VALUES (@Id, @AppId, @Name, @Code)";
        var parameters = new
        {
            Id = language.Id.Value,
            AppId = language.AppId.Value,
            Name = language.LanguageCode.Name,
            Code = language.LanguageCode.Code
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, parameters);
    }
}