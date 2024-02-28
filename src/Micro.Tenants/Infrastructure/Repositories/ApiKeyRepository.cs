using System.Data;
using Dapper;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application.ApiKeys;
using Micro.Tenants.Domain.ApiKeys;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Infrastructure.Repositories;

internal class ApiKeyRepository(ConnectionFactory connections) : IApiKeyRepository
{
    public async Task CreateAsync(UserApiKey key, CancellationToken token)
    {
        const string sql = $"INSERT INTO {UserApiKeysTable} ({IdColumn}, {UserIdColumn}, {NameColumn}, {KeyColumn}, {CreatedAtColumn}) " +
                           "VALUES (@Id, @UserId, @Name, @Key, @CreatedAt)";
        using var con = connections.CreateConnection();
        var parameters = new
        {
            Id = key.Id.Value,
            UserId = key.UserId.Value,
            Name = key.ApiKey.Name.Name,
            Key = key.ApiKey.Key.Value,
            CreatedAt = key.ApiKey.CreatedAt
        };
        await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: token));
    }

    public async Task DeleteAsync(UserApiKeyId id, CancellationToken token)
    {
        const string sql = $"DELETE FROM {UserApiKeysTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var parameters = new
        {
            id
        };
        await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: token));
    }

    public async Task<UserApiKey?> GetById(UserApiKeyId id, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {UserIdColumn}, {NameColumn}, {KeyColumn}, {CreatedAtColumn} " +
                           $"FROM {UserApiKeysTable} WHERE {IdColumn} = @Id";
        using var con = connections.CreateConnection();
        var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
        {
            id
        }, cancellationToken: token));
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<UserApiKey?> GetByName(UserId userId, ApiKeyName name, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {UserIdColumn}, {NameColumn}, {KeyColumn}, {CreatedAtColumn} " +
                           $"FROM {UserApiKeysTable} WHERE {UserIdColumn} = @UserId AND {NameColumn} = @Name";
        using var con = connections.CreateConnection();
        var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
        {
            userId,
            name
        }, cancellationToken: token));
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<UserApiKey?> GetByKey(ApiKeyValue key, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {UserIdColumn}, {NameColumn}, {KeyColumn}, {CreatedAtColumn} " +
                           $"FROM {UserApiKeysTable} WHERE {KeyColumn} = @Key";
        using var con = connections.CreateConnection();
        var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new
        {
            Key = key
        }, cancellationToken: token));
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<IEnumerable<UserApiKey>> ListAsync(UserId userId, CancellationToken token)
    {
        const string sql = $"SELECT {IdColumn}, {UserIdColumn}, {NameColumn}, {KeyColumn}, {CreatedAtColumn} " +
                           $"FROM {UserApiKeysTable} WHERE {UserIdColumn} = @UserId";
        using var con = connections.CreateConnection();
        var reader = await con.ExecuteReaderAsync(new CommandDefinition(sql, new { userId }, cancellationToken: token));
        return MapMany(reader);
    }

    private static IEnumerable<UserApiKey> MapMany(IDataReader reader)
    {
        var list = new List<UserApiKey>();
        while (reader.Read())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    private static UserApiKey Map(IDataRecord reader)
    {
        var id = new UserApiKeyId(reader.GetGuid(0));
        var userId = new UserId(reader.GetGuid(1));
        var name = new ApiKeyName(reader.GetString(2));
        var key = new ApiKeyValue(reader.GetString(3));
        var createdAt = reader.GetDateTime(4);
        return UserApiKey.Create(id, userId, ApiKey.From(name, key, createdAt));
    }
}