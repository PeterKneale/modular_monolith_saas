using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common;
using Micro.Common.Infrastructure.Integration;
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
        services.AddMediatR(c => { c.RegisterServicesFromAssemblies(assemblies); });
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

        // Repositories
        services.AddScoped<ITermRepository, TermRepository>();

        // Database Migrations
        services
            .AddSingleton<IConventionSet>(new DefaultConventionSet(Constants.SchemaName, null))
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        // Database
        services.AddDbContext<Db>((ctx, options) =>
        {
            options.UseNpgsql(connectionString);
            options.UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>());
            // options.EnableSensitiveDataLogging();
            // options.EnableDetailedErrors();
        });

        // Inbox/Outbox
        services.AddScoped<IInboxDbSet>(c => c.GetRequiredService<Db>());
        services.AddScoped<IOutboxDbSet>(c => c.GetRequiredService<Db>());
        services.AddScoped<IQueueDbSet>(c => c.GetRequiredService<Db>());

        return services;
    }
}