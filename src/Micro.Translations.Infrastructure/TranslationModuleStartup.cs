using System.Collections.Specialized;
using System.Reflection;
using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Common.Infrastructure.Jobs;
using Micro.Tenants.Messages;
using Micro.Translations.Infrastructure.Integration;
using Micro.Translations.Infrastructure.Integration.Handlers;
using Micro.Users.Messages;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Micro.Translations.Infrastructure;

public static class TranslationModuleStartup
{
    private static IScheduler? _scheduler;

    public static async Task Start(IExecutionContextAccessor accessor, IConfiguration configuration, IEventsBus bus, ILoggerFactory logs, bool enableMigrations = true, bool resetDb = false, bool enableScheduler = true)
    {
        var serviceProvider = new ServiceCollection()
            .AddContextAccessor(accessor)
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(bus)
            .AddSingleton(logs)
            .BuildServiceProvider();

        bus.Subscribe<UserCreated>(new IntegrationEventHandler());
        bus.Subscribe<UserChanged>(new IntegrationEventHandler());
        bus.Subscribe<OrganisationCreated>(new IntegrationEventHandler());
        bus.Subscribe<OrganisationUpdated>(new IntegrationEventHandler());
        bus.Subscribe<ProjectCreated>(new IntegrationEventHandler());
        
        TranslationsCompositionRoot.SetProvider(serviceProvider);

        if (enableMigrations) serviceProvider.ApplyDatabaseMigrations(resetDb);
        if (enableScheduler) _scheduler = await SetupScheduledJobs();
    }

    public static async Task Stop()
    {
        if (_scheduler != null) await _scheduler.Shutdown();
    }

    private static async Task<IScheduler> SetupScheduledJobs()
    {
        LogProvider.SetCurrentLogProvider(new QuartzConsoleLogger());
        var factory = new StdSchedulerFactory(new NameValueCollection
        {
            { "quartz.scheduler.instanceName", Assembly.GetExecutingAssembly().GetName().Name }
        });
        var scheduler = await factory.GetScheduler();
        await scheduler.AddMessageboxJob<OutboxJob>();
        await scheduler.AddMessageboxJob<InboxJob>();
        await scheduler.AddMessageboxJob<QueueJob>();
        await scheduler.Start();
        return scheduler;
    }
}