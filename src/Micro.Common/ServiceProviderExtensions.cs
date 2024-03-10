using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Micro.Common;

public static class ServiceProviderExtensions
{
    private const int RetryAttempts = 10;
    private static readonly TimeSpan RetryInterval = TimeSpan.FromSeconds(1);

    public static IServiceProvider ApplyDatabaseMigrations(this IServiceProvider app, bool reset = false)
    {
        var logs = app.GetRequiredService<ILogger<IMigrationRunner>>();

        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                RetryAttempts,
                retryAttempt => RetryInterval,
                (exception, timeSpan, attempt, context) =>
                    logs.LogWarning($"Attempt {attempt} of {RetryAttempts} failed with exception {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));

        policy.Execute(() =>
        {
            using var scope = app.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            logs.LogInformation("Migrating database schema");
            if (reset) runner.MigrateDown(0);

            runner.MigrateUp();
            logs.LogInformation("Migrated database schema");
        });

        return app;
    }
}