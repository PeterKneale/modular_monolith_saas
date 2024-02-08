using Dapper;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Domain;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Users;
using static Micro.Tenants.Infrastructure.Database.Constants;

namespace Micro.Tenants.Infrastructure.Database;

internal class UserRepository(ConnectionFactory connections) : IUserRepository
{
    public async Task CreateAsync(User user)
    {
        const string sql = $"INSERT INTO {SchemaName}.Users (id, organisation_id, first_name, last_name, email, password, role) " +
                           "VALUES (@Id, @OrganisationId, @FirstName, @LastName, @Email, @Password, @Role)";
        using var con = connections.CreateConnection();
        var row = new Row
        {
            Id = user.Id.Value,
            OrganisationId = user.OrganisationId.Value,
            FirstName = user.Name.First,
            LastName = user.Name.Last,
            Email = user.Credentials.Email,
            Password = user.Credentials.Password,
            Role = user.Role.Name
        };
        await con.ExecuteAsync(sql, row);
    }

    public async Task<User?> GetAsync(UserId id)
    {
        const string sql = $"SELECT * FROM {SchemaName}.Users WHERE id = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(sql, new { Id = id.Value });
        return row == null ? null : Map(row);
    }

    private static User Map(Row row)
    {
        var organisationId = new OrganisationId(row.OrganisationId);
        var userId = new UserId(row.Id);
        var name = new UserName(row.FirstName, row.LastName);
        var credentials = new UserCredentials(row.Email, row.Password);
        var role = new UserRole(row.Role);
        return new User(organisationId, userId, name, credentials, role);
    }

    private class Row
    {
        public Guid Id { get; init; }
        public Guid OrganisationId { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}