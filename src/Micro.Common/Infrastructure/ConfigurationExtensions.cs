using Microsoft.Extensions.Configuration;

namespace Micro.Common.Infrastructure;

public static class ConfigurationExtensions
{
    public static bool IsSchedulerEnabled(this IConfiguration configuration)
    {
        const string key = "SCHEDULER_ENABLED";
        var enabled = configuration[key] ?? bool.TrueString;
        return bool.Parse(enabled);
    }

    public static bool IsMigrationEnabled(this IConfiguration configuration)
    {
        const string key = "MIGRATIONS_ENABLED";
        var enabled = configuration[key] ?? bool.TrueString;
        return bool.Parse(enabled);
    }
}