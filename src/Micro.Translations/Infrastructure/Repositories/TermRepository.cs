using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;
using static Micro.Translations.Constants;

namespace Micro.Translations.Infrastructure.Repositories;

internal class TermRepository(ConnectionFactory connections) : ITermRepository
{
    public async Task CreateAsync(Term term, CancellationToken token)
    {
        const string sql = $"INSERT INTO {TermsTable} ({IdColumn},{ProjectIdColumn},{NameColumn}) VALUES (@Id, @ProjectId, @Name)";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new
        {
            term.Id,
            term.ProjectId,
            term.Name
        }, cancellationToken: token));
    }

    public async Task UpdateAsync(Term term, CancellationToken token)
    {
        const string sql = $"UPDATE {TermsTable} SET {NameColumn} = @Name WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new
        {
            term.Id,
            term.Name
        }, cancellationToken: token));
    }

    public async Task<Term?> GetAsync(TermId id, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn},{ProjectIdColumn},{NameColumn} FROM {TermsTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var row = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { id }, cancellationToken: token));
        return row.Read() ? Map(row) : null;
    }

    private static Term Map(IDataRecord record)
    {
        var id = new TermId(record.GetGuid(0));
        var projectId = new ProjectId(record.GetGuid(1));
        var name = new TermName(record.GetString(2));
        return new Term(id, projectId, name);
    }
}