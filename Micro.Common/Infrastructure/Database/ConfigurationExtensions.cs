using Microsoft.Extensions.Configuration;

namespace Micro.Common.Infrastructure.Database;

public static class ConfigurationExtensions
{
    private const string Template = "Username={0};Password={1};Database={2};Host={3};Port={4};Include Error Detail=true;Log Parameters=true";

    public static string GetDbConnectionString(this IConfiguration configuration)
    {
        var username = configuration["DB_USERNAME"] ?? "admin";
        var password = configuration["DB_PASSWORD"] ?? "password";
        var database = configuration["DB_DATABASE"] ?? "db";
        var host = configuration["DB_HOST"] ?? "localhost";
        var port = configuration["DB_PORT"] ?? "5432";
        var connectionString = string.Format(Template, username, password, database, host, port);
        return connectionString;
    }
}