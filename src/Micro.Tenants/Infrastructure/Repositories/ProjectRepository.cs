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
        var row = new Row
        {
            Id = project.Id.Value,
            OrganisationId = project.OrganisationId.Value,
            Name = project.Name.Value
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, row, cancellationToken: token));
    }

    public async Task UpdateAsync(Project project, CancellationToken token)
    {
        const string sql = $"UPDATE {ProjectsTable} SET {NameColumn} = @Name WHERE {IdColumn} = @Id";
        var row = new Row
        {
            Id = project.Id.Value,
            Name = project.Name.Value
        };
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, row, cancellationToken: token));
    }

    public async Task<Project?> GetAsync(ProjectId id, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {ProjectsTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: token));
        return row == null ? null : Map(row);
    }

    public async Task<Project?> GetAsync(ProjectName name, CancellationToken token)
    {
        const string sql = $"SELECT * FROM {ProjectsTable} WHERE {NameColumn} = @Name";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new { Name = name.Value }, cancellationToken: token));
        return row == null ? null : Map(row);
    }

    private static Project Map(Row row)
    {
        var id = new ProjectId(row.Id);
        var name = new ProjectName(row.Name);
        var organisationId = new OrganisationId(row.OrganisationId);
        return new Project(id, organisationId, name);
    }

    private class Row
    {
        public Guid Id { get; init; }
        public Guid OrganisationId { get; init; }
        public string Name { get; init; } = null!;
    }
}