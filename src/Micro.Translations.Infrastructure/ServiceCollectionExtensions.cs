using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common;
using Micro.Common.Infrastructure.Integration;
using Micro.Translations.Infrastructure.Database;
using Micro.Translations.Infrastructure.Database.Repositories;
using Micro.Translations.Infrastructure.Database.TypeHandlers;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Micro.Translations.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString(Constants.SchemaName);
        if (string.IsNullOrWhiteSpace(connectionString)) throw new Exception("Connection string missing");

        var assemblies = new[]
        {
            InfrastructureAssemblyInfo.Assembly,
            ApplicationAssemblyInfo.Assembly,
            CommonAssemblyInfo.Assembly
        };
        services.AddMediatR(c => { c.RegisterServicesFromAssemblies(assemblies); });
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

        // Repositories
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<ITermRepository, TermRepository>();
        
        // dapper
        services.AddScoped<IDbConnection>(c => new NpgsqlConnection(connectionString));
        SqlMapper.AddTypeHandler(LanguageIdTypeHandler.Default);
        
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
        services.AddScoped<IDbSetInbox>(c => c.GetRequiredService<Db>());
        services.AddScoped<IDbSetOutbox>(c => c.GetRequiredService<Db>());
        services.AddScoped<IDbSetQueue>(c => c.GetRequiredService<Db>());

        return services;
    }
}