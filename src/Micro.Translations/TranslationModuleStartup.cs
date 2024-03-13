using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Tenants.IntegrationEvents;
using Micro.Translations.Infrastructure;
using Micro.Translations.Infrastructure.Integration;
using Microsoft.Extensions.Configuration;

namespace Micro.Translations;

public static class TranslationModuleStartup
{
    public static void Start(IExecutionContextAccessor accessor, IConfiguration configuration, IEventsBus bus,ILoggerFactory logs, bool resetDb = false)
    {
        var serviceProvider = new ServiceCollection()
            .AddContextAccessor(accessor)
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(bus)
            .AddSingleton(logs)
            .BuildServiceProvider()
            .ApplyDatabaseMigrations(resetDb);

        bus.Subscribe<OrganisationCreated>(new IntegrationEventHandler());
        bus.Subscribe<OrganisationChanged>(new IntegrationEventHandler());
        bus.Subscribe<UserCreated>(new IntegrationEventHandler());
        bus.Subscribe<UserChanged>(new IntegrationEventHandler());
        CompositionRoot.SetProvider(serviceProvider);
    }
}