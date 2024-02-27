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
        await con.ExecuteAsync(new CommandDefinition(sql, new Row
        {
            Id = organisation.Id,
            Name = organisation.Name
        }, cancellationToken: token));
    }

    public async Task UpdateAsync(Organisation organisation, CancellationToken token)
    {
        const string sql = $"UPDATE {OrganisationsTable} SET {NameColumn} = @Name WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new Row
        {
            Id = organisation.Id,
            Name = organisation.Name
        }, cancellationToken: token));
    }

    public async Task<Organisation?> GetAsync(OrganisationId id, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {NameColumn} FROM {OrganisationsTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new Row
        {
            Id = id
        }, cancellationToken: token));
        return Map(row);
    }

    public async Task<Organisation?> GetAsync(OrganisationName name, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {NameColumn} FROM {OrganisationsTable} WHERE {NameColumn} = @Name";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new Row
        {
            Name = name
        }, cancellationToken: token));
        return Map(row);
    }

    private static Organisation? Map(Row? row) => 
        row != null 
            ? new Organisation(row.Id, row.Name) 
            : null;

    private class Row
    {
        public OrganisationId Id { get; init; } = null!;
        public OrganisationName Name { get; init; } = null!;
    }
}