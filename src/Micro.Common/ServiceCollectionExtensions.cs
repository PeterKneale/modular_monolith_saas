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
        services.AddScoped<ICurrentContext>(_ =>
        {
            var context = accessor.CurrentContext;
            if (context == null)
            {
                throw new Exception("No context available");
            }
            return context;
        });
        return services;
    }
}