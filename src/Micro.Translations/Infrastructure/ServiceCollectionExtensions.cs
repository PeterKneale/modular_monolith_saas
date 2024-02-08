using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common.Infrastructure.Database;
using Micro.Translations.Application;
using Micro.Translations.Infrastructure.Database;
using Microsoft.Extensions.Configuration;

namespace Micro.Translations.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Connection string missing");
        }
        
        // application
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);
        
        // Connections
        services.AddSingleton<ConnectionFactory>(_=> new ConnectionFactory(connectionString));
        
        // Repositories
        services.AddSingleton<ITermRepository, TermRepository>();
        services.AddSingleton<ITranslationRepository, TranslationRepository>();
        
        // Database Migrations
        services
            .AddSingleton<IConventionSet>(new DefaultConventionSet(Constants.Schema, null))
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        return services;
    }
}