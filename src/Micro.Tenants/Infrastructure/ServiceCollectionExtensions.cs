using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Micro.Common.Infrastructure.Database;
using Micro.Tenants.Application;
using Micro.Tenants.Domain.Organisations;
using Micro.Tenants.Infrastructure.Database;
using Micro.Tenants.Infrastructure.Services;
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
        services.AddSingleton<ConnectionFactory>(_=> new ConnectionFactory(connectionString));
        
        // Services
        services.AddScoped<IOrganisationNameCheck, OrganisationNameCheck>();
        
        // Repositories
        services.AddSingleton<IOrganisationRepository, OrganisationRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IMembershipRepository, MembershipRepository>();
        
        // Database Migrations
        
        services
            .AddSingleton<IConventionSet>(new DefaultConventionSet(Constants.SchemaName, null))
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        return services;
    }
}