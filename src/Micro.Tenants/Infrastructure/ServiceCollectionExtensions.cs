using System.Reflection;
using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common;
using Micro.Common.Infrastructure.Database;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Tenants.Application.ApiKeys;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Projects;
using Micro.Tenants.Application.Users;
using Micro.Tenants.Infrastructure.Database;
using Micro.Tenants.Infrastructure.Database.Repositories;
using Micro.Tenants.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Micro.Tenants.Infrastructure;

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

        // Connections
        services.AddSingleton<ConnectionFactory>(_ => new ConnectionFactory(connectionString));

        // Services
        services.AddSingleton<IOrganisationNameCheck, OrganisationNameCheck>();
        services.AddSingleton<IProjectNameCheck, ProjectNameCheck>();
        services.AddSingleton<IApiKeyService, ApiKeyService>();

        // Repositories
        services.AddScoped<IOrganisationRepository, OrganisationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        // Inbox/Outbox
        services.AddScoped<IInboxRepository, InboxRepository>();
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

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }
}