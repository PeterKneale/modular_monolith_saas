using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Translations.Application;
using Micro.Translations.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Configuration;

namespace Micro.Translations.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString)) throw new Exception("Connection string missing");

        // application
        var assemblies = new[] { Assembly.GetExecutingAssembly(), CommonAssemblyInfo.Assembly };
        services.AddMediatR(assemblies);
        services.AddValidatorsFromAssemblies(assemblies);

        // Repositories
        services.AddScoped<ITermRepository, TermRepository>();

        // Inbox/Outbox
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        // Database Migrations
        services
            .AddSingleton<IConventionSet>(new DefaultConventionSet(Constants.SchemaName, null))
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        services.AddDbContext<Db>((ctx, options) =>
        {
            options.UseNpgsql(connectionString);
            options.UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>());
            // options.EnableSensitiveDataLogging();
            // options.EnableDetailedErrors();
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
        return services;
    }
}