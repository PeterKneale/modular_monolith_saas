﻿using System.Reflection;
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
        if (string.IsNullOrWhiteSpace(connectionString)) throw new Exception("Connection string missing");

        // application
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);

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

        services.AddDbContext<Db>((ctx, options) =>
        {
            options.UseNpgsql(connectionString);
            options.UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>());
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
        return services;
    }
}