using System.Reflection;
using Dapper;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application.ApiKeys;
using Micro.Tenants.Application.Memberships;
using Micro.Tenants.Application.Organisations;
using Micro.Tenants.Application.Projects;
using Micro.Tenants.Application.Users;
using Micro.Tenants.Infrastructure.Behaviours;
using Micro.Tenants.Infrastructure.DapperTypeHandlers;
using Micro.Tenants.Infrastructure.Ef;
using Micro.Tenants.Infrastructure.Repositories;
using Micro.Tenants.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Tenants.Infrastructure;

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

        // Services
        services.AddSingleton<IOrganisationNameCheck, OrganisationNameCheck>();
        services.AddSingleton<IProjectNameCheck, ProjectNameCheck>();
        services.AddSingleton<IApiKeyService, ApiKeyService>();

        // Repositories
        services.AddSingleton<IOrganisationRepository, OrganisationRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IApiKeyRepository, ApiKeyRepository>();
        services.AddSingleton<IMembershipRepository, MembershipRepository>();
        services.AddSingleton<IProjectRepository, ProjectRepository>();

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
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
        
        // Infrastructure
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(UserApiKeyIdTypeHandler.Default);
        SqlMapper.AddTypeHandler(ApiKeyNameTypeHandler.Default);
        SqlMapper.AddTypeHandler(ApiKeyValueTypeHandler.Default);
        SqlMapper.AddTypeHandler(OrganisationNameTypeHandler.Default);
        SqlMapper.AddTypeHandler(ProjectNameTypeHandler.Default);
        SqlMapper.AddTypeHandler(MembershipIdTypeHandler.Default);
        SqlMapper.AddTypeHandler(MembershipRoleTypeHandler.Default);
        return services;
    }
}