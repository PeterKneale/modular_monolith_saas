using System.Data;
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
        const string sql = $"SELECT {IdColumn}, {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn} " +
                           $"FROM {UsersTable} WHERE {IdColumn} = @Id";

        using var con = connections.CreateConnection();
        var row = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { id }, cancellationToken: token));
        return row.Read() ? Map(row) : null;
    }

    public async Task<User?> GetAsync(string email, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn} " +
                           $"FROM {UsersTable} WHERE {EmailColumn} = @Email";

        using var con = connections.CreateConnection();
        var row = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { Email = email }, cancellationToken: token));
        return row.Read() ? Map(row) : null;
    }

    private static User Map(IDataRecord row)
    {
        var id = row.GetGuid(0);
        var first = row.GetString(1);
        var last = row.GetString(2);
        var email = row.GetString(3);
        var password = row.GetString(4);
        var userId = new UserId(id);
        var name = new UserName(first, last);
        var credentials = new UserCredentials(email, password);
        return new User(userId, name, credentials);
    }
}