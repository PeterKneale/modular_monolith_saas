using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Translations.Application;
using Micro.Translations.Infrastructure.Behaviours;
using Micro.Translations.Infrastructure.Database;
using Micro.Translations.Infrastructure.Repositories;
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
        services.AddSingleton<ConnectionFactory>(_ => new ConnectionFactory(connectionString));

        // Repositories
        services.AddSingleton<ITermRepository, TermRepository>();
        services.AddSingleton<ITranslationRepository, TranslationRepository>();
        services.AddSingleton<ILanguageRepository, LanguageRepository>();

        // Database Migrations
        services
            .AddSingleton<IConventionSet>(new DefaultConventionSet(Constants.SchemaName, null))
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        services.AddDbContext<Db>(options => { options.UseNpgsql(connectionString); });

        // Infrastructure
        // SqlMapper.AddTypeHandler(LanguageIdTypeHandler.Default);
        // SqlMapper.AddTypeHandler(LanguageCodeTypeHandler.Default);
        // SqlMapper.AddTypeHandler(TermIdTypeHandler.Default);
        // SqlMapper.AddTypeHandler(TermNameTypeHandler.Default);
        // SqlMapper.AddTypeHandler(TranslationIdTypeHandler.Default);
        // SqlMapper.AddTypeHandler(TranslationTextTypeHandler.Default);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalBehaviour<,>));
        return services;
    }
}