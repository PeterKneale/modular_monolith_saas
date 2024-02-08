using Dapper;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Domain.Organisations;
using static Micro.Tenants.Infrastructure.Database.Constants;

namespace Micro.Tenants.Infrastructure.Database;

internal class OrganisationRepository(ConnectionFactory connections) : IOrganisationRepository
{
    public async Task CreateAsync(Organisation organisation)
    {
        const string sql = $"INSERT INTO {SchemaName}.organisations (id, name) VALUES (@Id, @Name)";
        var row = new Row
        {
            Id = organisation.Id.Value,
            Name = organisation.Name.Value
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, row);
    }

    public async Task UpdateAsync(Organisation organisation)
    {
        const string sql = $"UPDATE {SchemaName}.organisations SET name = @Name WHERE id = @Id";
        var row = new Row
        {
            Id = organisation.Id.Value,
            Name = organisation.Name.Value
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(sql, row);
    }

    public async Task<Organisation?> GetAsync(OrganisationId id)
    {
        const string sql = $"SELECT * FROM {SchemaName}.organisations WHERE id = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(sql, new { Id = id.Value });
        return row == null ? null : Map(row);
    }

    public async Task<Organisation?> GetAsync(OrganisationName name)
    {
        const string sql = $"SELECT * FROM {SchemaName}.organisations WHERE name = @Name";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(sql, new { Name = name.Value });
        return row == null ? null : Map(row);
    }

    private static Organisation Map(Row row)
    {
        var id = new OrganisationId(row.Id);
        var name = new OrganisationName(row.Name);
        return new Organisation(id, name);
    }

    private class Row
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
    }
}