using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Micro.IntegrationTests.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestLogging(this IServiceCollection services, ITestOutputHelperAccessor output)
    {
        services.AddLogging(builder => builder.AddXUnit(output, c =>
        {
            c.Filter = (category, level) =>
            {
                if (category.Contains("Microsoft.EntityFrameworkCore"))
                {
                    return level >= LogLevel.Warning;
                }

                return level >= LogLevel.Information;
            };
        }));
        return services;
    }
}
