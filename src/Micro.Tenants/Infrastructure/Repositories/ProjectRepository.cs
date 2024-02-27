using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application.Projects;
using Micro.Tenants.Domain.Projects;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Repositories;

internal class ProjectRepository(ConnectionFactory connections) : IProjectRepository
{
    public async Task CreateAsync(Project project, CancellationToken token)
    {
        const string sql = $"INSERT INTO {ProjectsTable} ({IdColumn}, {OrganisationIdColumn}, {NameColumn}) VALUES (@Id, @OrganisationId, @Name)";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new Row
        {
            Id = project.Id,
            OrganisationId = project.OrganisationId,
            Name = project.Name
        }, cancellationToken: token));
    }

    public async Task UpdateAsync(Project project, CancellationToken token)
    {
        const string sql = $"UPDATE {ProjectsTable} SET {NameColumn} = @Name WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new Row
        {
            Id = project.Id,
            Name = project.Name
        }, cancellationToken: token));
    }

    public async Task<Project?> GetAsync(ProjectId id, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {ProjectsTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new Row
        {
            Id = id
        }, cancellationToken: token));
        return Map(row);
    }

    public async Task<Project?> GetAsync(ProjectName name, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {ProjectsTable} WHERE {NameColumn} = @Name";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new Row
        {
            Name = name
        }, cancellationToken: token));
        return Map(row);
    }

    private static Project? Map(Row? row) =>
        row != null
            ? new Project(row.Id, row.OrganisationId, row.Name)
            : null;

    private class Row
    {
        public ProjectId Id { get; init; } = null!;
        public OrganisationId OrganisationId { get; init; } = null!;
        public ProjectName Name { get; init; } = null!;
    }
}