using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Tenants.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Tenants;

public static class TenantsModuleStartup
{
    public static void Start(IContextAccessor accessor, IConfiguration configuration, ILoggerFactory logs, bool resetDb = false)
    {
        var serviceProvider = new ServiceCollection()
            .AddContextAccessor(accessor)
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(logs)
            .BuildServiceProvider()
            .ApplyDatabaseMigrations(resetDb);

        CompositionRoot.SetProvider(serviceProvider);
    }
}