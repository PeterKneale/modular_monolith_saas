using System.Reflection;
using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Infrastructure.Database;
using Micro.Tenants.Infrastructure.Database.Repositories;
using Micro.Tenants.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Micro.Tenants.Infrastructure;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString(DbConstants.SchemaName);
        if (string.IsNullOrWhiteSpace(connectionString)) throw new Exception("Connection string missing");

        // application
        var assemblies = new[]
        {
            InfrastructureAssemblyInfo.Assembly,
            ApplicationAssemblyInfo.Assembly,
            CommonAssemblyInfo.Assembly
        };
        services.AddMediatR(c => { c.RegisterServicesFromAssemblies(assemblies); });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

        services.AddValidatorsFromAssemblies(assemblies);

        // Connections
        services.AddSingleton<ConnectionFactory>(_ => new ConnectionFactory(connectionString));

        // Services
        services.AddScoped<IOrganisationNameCheck, OrganisationNameCheck>();

        // Repositories
        services.AddScoped<IOrganisationRepository, OrganisationRepository>();

        // Database Migrations
        services
            .AddSingleton<IConventionSet>(new DefaultConventionSet(DbConstants.SchemaName, null))
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
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        // Inbox/Outbox
        services.AddScoped<IDbSetInbox>(c => c.GetRequiredService<Db>());
        services.AddScoped<IDbSetOutbox>(c => c.GetRequiredService<Db>());
        services.AddScoped<IDbSetQueue>(c => c.GetRequiredService<Db>());

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }
}