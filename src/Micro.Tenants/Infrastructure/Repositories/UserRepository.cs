using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Application.Users;
using Micro.Tenants.Domain.Users;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Repositories;

internal class UserRepository(ConnectionFactory connections) : IUserRepository
{
    public async Task CreateAsync(User user, CancellationToken token)
    {
        const string sql = $"INSERT INTO {UsersTable} ({IdColumn}, {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn}) " +
                           "VALUES (@Id, @FirstName, @LastName, @Email, @Password)";
        using var con = connections.CreateConnection();
        await con.ExecuteAsync(new CommandDefinition(sql, new
        {
            Id = user.Id.Value,
            FirstName = user.Name.First,
            LastName = user.Name.Last,
            Email = user.Credentials.Email,
            Password = user.Credentials.Password,
        }, cancellationToken: token));
    }

    public async Task<User?> GetAsync(UserId id, CancellationToken token)
    {
        const string sql = $"SELECT {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn} " +
                           $"FROM {UsersTable} WHERE {IdColumn} = @Id";

        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync(new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: token));
        if (row == null)
            return null;
        var name = new UserName(row.FirstName, row.LastName);
        var credentials = new UserCredentials(row.Email, row.Password);
        return new User(id, name, credentials);
    }

    public async Task<User?> GetAsync(string email, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn} " +
                           $"FROM {UsersTable} WHERE {EmailColumn} = @Email";

        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync(new CommandDefinition(sql, new { Email = email }, cancellationToken: token));
        if (row == null)
            return null;
        var id = new UserId(Guid.Parse(row.id.ToString()));
        var name = new UserName(row.first_name, row.last_name);
        var credentials = new UserCredentials(row.email, row.password);
        return new User(id, name, credentials);
    }
}