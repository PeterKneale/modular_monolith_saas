using Dapper;
using Micro.Common.Infrastructure.Database;
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
        await con.ExecuteAsync(new CommandDefinition(sql, new Row
        {
            Id = user.Id,
            FirstName = user.Name.First,
            LastName = user.Name.First,
            Email = user.Credentials.Email,
            Password = user.Credentials.Password
        }, cancellationToken: token));
    }

    public async Task<User?> GetAsync(UserId id, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn} " +
                           $"FROM {UsersTable} WHERE {IdColumn} = @Id";

        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new
        {
            id
        }, cancellationToken: token));
        return Map(row);
    }

    public async Task<User?> GetAsync(EmailAddress email, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {FirstNameColumn}, {LastNameColumn}, {EmailColumn}, {PasswordColumn} " +
                           $"FROM {UsersTable} WHERE {EmailColumn} = @Email";

        using var con = connections.CreateConnection();
        var row = await con.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new
        {
            Email = email
        }, cancellationToken: token));
        return Map(row);
    }

    private static User? Map(Row? row) =>
        row != null
            ? User.CreateInstance(row.Id, new UserName(row.FirstName, row.LastName), new UserCredentials(row.Email, row.Password))
            : null;

    public class Row
    {
        public UserId Id { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public EmailAddress Email { get; init; } = null!;
        public Password Password { get; init; } = null!;
    }
}