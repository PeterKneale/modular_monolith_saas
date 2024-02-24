using Micro.Common.Infrastructure.Database;
using Micro.Translations.Application;
using Micro.Translations.Domain.Languages;

namespace Micro.Translations.Infrastructure.Repositories;

internal class LanguageRepository(ConnectionFactory connections) : ILanguageRepository
{
    public async Task CreateAsync(ResultLanguage resultLanguage)
    {
        const string sql = $"INSERT INTO {Constants.LanguagesTable} (id, project_id, name, code) VALUES (@Id, @ProjectId, @Name, @Code)";
        var parameters = new
        {
            Id = resultLanguage.Id.Value,
            ProjectId = resultLanguage.ProjectId.Value,
            Name = resultLanguage.LanguageCode.Name,
            Code = resultLanguage.LanguageCode.Code
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, parameters);
    }
}