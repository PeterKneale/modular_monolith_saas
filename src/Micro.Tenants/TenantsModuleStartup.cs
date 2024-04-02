using System.Collections.Specialized;
using System.Reflection;
using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Common.Infrastructure.Jobs;
using Micro.Tenants.Infrastructure;
using Micro.Tenants.Infrastructure.Integration;
using Micro.Tenants.Infrastructure.Integration.Handlers;
using Micro.Users.IntegrationEvents;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Micro.Tenants;

public static class TenantsModuleStartup
{
    private static IScheduler? _scheduler;

    public static async Task Start(IExecutionContextAccessor accessor, IConfiguration configuration, IEventsBus bus, ILoggerFactory logs, bool resetDb = false, bool enableScheduler = true)
    {
        var serviceProvider = new ServiceCollection()
            .AddContextAccessor(accessor)
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(bus)
            .AddSingleton(logs)
            .BuildServiceProvider()
            .ApplyDatabaseMigrations(resetDb);

        bus.Subscribe<UserCreated>(new IntegrationEventHandler());
        bus.Subscribe<UserChanged>(new IntegrationEventHandler());

        CompositionRoot.SetProvider(serviceProvider);

        if (enableScheduler) _scheduler = await SetupScheduledJobs();
    }

    public static async Task Stop()
    {
        if (_scheduler != null)
            await _scheduler.Shutdown();
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