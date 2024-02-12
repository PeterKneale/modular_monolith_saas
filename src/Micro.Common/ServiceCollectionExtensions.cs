using Micro.Common.Application;
using Micro.Common.Infrastructure.Behaviours;
using Micro.Common.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        return services;
    }

    public static IServiceCollection AddContextAccessor(this IServiceCollection services, IContextAccessor accessor)
    {
        services.AddScoped<IUserContext>(_ => accessor.User ?? throw new Exception("No user context available"));
        services.AddScoped<IOrganisationContext>(_ => accessor.Organisation ?? throw new Exception("No organisation context available"));
        services.AddScoped<IProjectContext>(_ => accessor.Project ?? throw new Exception("No app context available"));
        return services;
    }
}