using Dapper;
using Micro.Common.Application;
using Micro.Common.Infrastructure.Behaviours;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Dapper;
using Micro.Common.Infrastructure.DomainEvents;
using Micro.Common.Infrastructure.Integration.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        SqlMapper.AddTypeHandler(OrganisationIdTypeHandler.Default);
        SqlMapper.AddTypeHandler(UserIdTypeHandler.Default);
        SqlMapper.AddTypeHandler(ProjectIdTypeHandler.Default);
        SqlMapper.AddTypeHandler(EmailAddressTypeHandler.Default);
        SqlMapper.AddTypeHandler(PasswordTypeHandler.Default);
        
        // domain events
        services.AddScoped<DomainEventAccessor>();
        services.AddScoped<DomainEventPublisher>();

        services.AddScoped<OutboxMessagePublisher>();
        return services;
    }

    public static IServiceCollection AddContextAccessor(this IServiceCollection services, IExecutionContextAccessor accessor)
    {
        services.AddScoped<IExecutionContext>(_ => accessor.ExecutionContext);
        return services;
    }
}