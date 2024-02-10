using Dapper;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Domain;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Domain.Users;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Database;

internal class UserRepository(ConnectionFactory connections) : IUserRepository
{
    public async Task CreateAsync(User user)
    {
        const string sql = $"INSERT INTO {UsersTable} (id, first_name, last_name, email, password) " +
                           "VALUES (@Id, @FirstName, @LastName, @Email, @Password)";
        using var con = connections.CreateConnection();
        var row = new Row
        {
            Id = user.Id.Value,
            FirstName = user.Name.First,
            LastName = user.Name.Last,
            Email = user.Credentials.Email,
            Password = user.Credentials.Password,
        };
        await con.ExecuteAsync(sql, row);
    }

    public async Task<User?> GetAsync(UserId id)
    {
        const string sql = $"SELECT * FROM {UsersTable} WHERE id = @Id";
        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(sql, new { Id = id.Value });
        if (row == null)
            return null;
        var name = new UserName(row.FirstName, row.LastName);
        var credentials = new UserCredentials(row.Email, row.Password);
        return new User(id, name, credentials);
    }

    private class Row
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}