using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Translations;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Repositories;

internal class TranslationRepository(ConnectionFactory connections) : ITranslationRepository
{
    public async Task CreateAsync(Translation translation, CancellationToken token)
    {
        const string sql = $"INSERT INTO {Schema}.translations (id, term_id, language_code, text) VALUES (@Id, @TermId, @LanguageCode, @Text)";

        var parameters = new
        {
            Id = translation.Id.Value,
            TermId = translation.TermId.Value,
            LanguageCode = translation.Language.Code,
            Text = translation.Text.Value
        };

        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: token));
    }

    public async Task UpdateAsync(Translation translation, CancellationToken token)
    {
        const string sql = $"UPDATE {Schema}.translations SET text = @Text WHERE id = @Id";

        var parameters = new
        {
            Id = translation.Id.Value,
            Text = translation.Text.Value
        };

        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, parameters);
    }

    public async Task<Translation?> GetAsync(TranslationId id, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {Schema}.translations WHERE id = @Id";

        var parameters = new { Id = id.Value };

        using var con = connections.CreateConnection();
        var result = await con.QuerySingleOrDefaultAsync(new CommandDefinition(sql, parameters, cancellationToken: token));
        
        if (result == null)
        {
            return null;
        }
        
        var termId = new TermId(result.TermId);
        var languageCode = LanguageCode.FromIsoCode(result.LanguageCode);
        var text = new Text(result.Text);
        return new Translation(id, termId, languageCode, text);
    }
}