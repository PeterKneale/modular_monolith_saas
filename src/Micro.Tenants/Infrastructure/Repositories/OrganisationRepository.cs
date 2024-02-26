using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Domain.Organisations;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Repositories;

internal class OrganisationRepository(ConnectionFactory connections) : IOrganisationRepository
{
    public async Task CreateAsync(Organisation organisation, CancellationToken token)
    {
        const string sql = $"INSERT INTO {OrganisationsTable} ({IdColumn}, {NameColumn}) VALUES (@Id, @Name)";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new
        {
            organisation.Id,
            organisation.Name
        }, cancellationToken: token));
    }

    public async Task UpdateAsync(Organisation organisation, CancellationToken token)
    {
        const string sql = $"UPDATE {OrganisationsTable} SET {NameColumn} = @Name WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new
        {
            organisation.Id,
            organisation.Name
        }, cancellationToken: token));
    }

    public async Task<Organisation?> GetAsync(OrganisationId id, CancellationToken token)
    {
        const string sql = $"SELECT {NameColumn} FROM {OrganisationsTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var name = await con.QuerySingleAsync<string>(new CommandDefinition(sql, new
        {
            id
        }, cancellationToken: token));
        return new Organisation(id, new OrganisationName(name));
    }

    public async Task<Organisation?> GetAsync(OrganisationName name, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {OrganisationsTable} WHERE {NameColumn} = @Name";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new
        {
            name
        }, cancellationToken: token));
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