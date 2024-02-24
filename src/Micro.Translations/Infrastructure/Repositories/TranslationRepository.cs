using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Translations;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Repositories;

internal class TranslationRepository(ConnectionFactory connections) : ITranslationRepository
{
    public async Task CreateAsync(Translation translation, CancellationToken token)
    {
        const string sql = $"INSERT INTO {Schema}.translations (id, term_id, language_id, text) VALUES (@Id, @TermId, @LanguageId, @TranslationText)";

        var parameters = new
        {
            translation.Id,
            translation.TermId,
            translation.LanguageId,
            translation.TranslationText
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
            Text = translation.TranslationText.Value
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
        var text = new TranslationText(result.Text);
        return new Translation(id, termId, languageCode, text);
    }
}