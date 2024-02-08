using System.Data;
using Npgsql;

namespace Micro.Common.Infrastructure.Database;

public class ConnectionFactory(string connectionString)
{
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
}