using Dapper;
using Micro.Common.Application;
using Micro.Common.Infrastructure.Behaviours;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Dapper;
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
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        return services;
    }

    public static IServiceCollection AddContextAccessor(this IServiceCollection services, IContextAccessor accessor)
    {
        services.AddScoped<IUserExecutionContext>(_ => accessor.User ?? throw new Exception("No user context available"));
        services.AddScoped<IOrganisationExecutionContext>(_ => accessor.Organisation ?? throw new Exception("No organisation context available"));
        services.AddScoped<IProjectExecutionContext>(_ => accessor.Project ?? throw new Exception("No project context available"));
        return services;
    }
}