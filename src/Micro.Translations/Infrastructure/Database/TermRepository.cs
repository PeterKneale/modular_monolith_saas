using Dapper;
using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Application;
using Micro.Translations.Domain;
using Micro.Translations.Domain.Terms;
using static Micro.Translations.Infrastructure.Database.Constants;

namespace Micro.Translations.Infrastructure.Database;

internal class TermRepository(ConnectionFactory connections, ICurrentContext context) : ITermRepository
{
    public async Task CreateAsync(Term term)
    {
        const string sql = $"INSERT INTO {SchemaName}.{TableNameTerms} (id, organisation_id, app_id, name) VALUES (@Id, @OrganisationId, @AppId, @Name)";
        var row = new Row
        {
            Id = term.Id.Value,
            OrganisationId = context.OrganisationId.Value,
            AppId = term.AppId.Value,
            Name = term.Name.Value
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, row);
    }

    public async Task UpdateAsync(Term term)
    {
        const string sql = $"UPDATE {SchemaName}.{TableNameTerms} SET name = @Name WHERE id = @Id";
        var row = new Row
        {
            Id = term.Id.Value,
            Name = term.Name.Value
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, row);
    }

    public async Task<Term?> GetAsync(TermId id)
    {
        const string sql = $"SELECT * FROM {SchemaName}.{TableNameTerms} WHERE id = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(sql, new { Id = id.Value });
        return row == null ? null : Map(row);
    }

    private static Term Map(Row row)
    {
        var id = new TermId(row.Id);
        var organisationId = new OrganisationId(row.OrganisationId);
        var appId = new AppId(row.AppId);
        var name = new TermName(row.Name);
        return new Term(id, organisationId, appId, name);
    }

    private class Row
    {
        public Guid Id { get; init; }
        public Guid OrganisationId { get; init; }
        public Guid AppId { get; init; }
        public string Name { get; init; } = null!;
    }
}