﻿using Dapper;
using Micro.Common.Application;
using Micro.Common.Infrastructure.Behaviours;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Dapper;
using Micro.Common.Infrastructure.DomainEvents;
using Micro.Common.Infrastructure.Integration.Inbox;
using Micro.Common.Infrastructure.Integration.Outbox;
using Micro.Common.Infrastructure.Integration.Queue;
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

        // domain events
        services.AddScoped<DomainEventAccessor>();
        services.AddScoped<DomainEventPublisher>();
        services.AddScoped<OutboxMessagePublisher>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // inbox
        services.AddScoped<InboxWriter>();
        services.AddScoped<InboxHandler>();

        // outbox
        services.AddScoped<OutboxWriter>();
        services.AddScoped<OutboxHandler>();

        // queue
        services.AddScoped<QueueWriter>();

        return services;
    }

    public static IServiceCollection AddContextAccessor(this IServiceCollection services, IExecutionContextAccessor accessor)
    {
        services.AddScoped<IExecutionContext>(_ => accessor.ExecutionContext);
        return services;
    }
}