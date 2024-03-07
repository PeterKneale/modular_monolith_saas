using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Translations.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Micro.Translations;

public static class TranslationModuleStartup
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