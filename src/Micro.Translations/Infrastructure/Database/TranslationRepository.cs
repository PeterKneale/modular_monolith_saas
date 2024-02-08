using Dapper;
using Micro.Common.Application;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Translations;

namespace Micro.Translations.Infrastructure.Database;

internal class TranslationRepository(ConnectionFactory connections, ICurrentContext context) : ITranslationRepository
{
    public async Task CreateAsync(Translation translation, CancellationToken token)
    {
        const string sql = $"INSERT INTO {Constants.SchemaName}.translations (id, term_id, language_code, text) VALUES (@Id, @TermId, @LanguageCode, @Text)";
        
        var parameters = new Row
        {
            Id = translation.Id.Value,
            TermId = translation.TermId.Value,
            LanguageCode = translation.Language.Name,
            Text = translation.Text.Value
        };
        
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: token));
    }

    public async Task UpdateAsync(Translation translation, CancellationToken token)
    {
        const string sql = $"UPDATE {Constants.SchemaName}.translations SET text = @Text WHERE id = @Id";
        
        var parameters = new Row
        {
            Id = translation.Id.Value,
            Text = translation.Text.Value
        };
        
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, parameters);
    }

    public async Task<Translation?> GetAsync(TranslationId id, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {Constants.SchemaName}.translations WHERE id = @Id";
        
        var parameters = new Row { Id = id.Value };
        
        using var con = connections.CreateConnection();
        var result = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, parameters, cancellationToken: token));
        
        return result == null 
            ? null 
            : Map(result);
    }

    private class Row
    {
        public Guid Id { get; init; }
        public Guid TermId { get; set; }
        public string LanguageCode { get; init; } = null!;
        public string Text { get; init; } = null!;
    }

    private static Translation Map(Row row)
    {
        var id = new TranslationId(row.Id);
        var termId = new TermId(row.TermId);
        var language = Language.FromName(row.LanguageCode);
        var text = new Text(row.Text);
        return new Translation(id, termId, language, text);
    }
}