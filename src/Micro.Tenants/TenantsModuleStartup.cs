using System.Collections.Specialized;
using System.Reflection;
using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Common.Infrastructure.Jobs;
using Micro.Tenants.Infrastructure;
using Micro.Tenants.Infrastructure.Integration;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Micro.Tenants;

public static class TenantsModuleStartup
{
    private static IScheduler _scheduler = null!;

    public static async Task Start(IExecutionContextAccessor accessor, IConfiguration configuration, IEventsBus bus, ILoggerFactory logs, bool resetDb = false)
    {
        var serviceProvider = new ServiceCollection()
            .AddContextAccessor(accessor)
            .AddCommon(configuration)
            .AddServices(configuration)
            .AddSingleton(bus)
            .AddSingleton(logs)
            .BuildServiceProvider()
            .ApplyDatabaseMigrations(resetDb);

        CompositionRoot.SetProvider(serviceProvider);

        _scheduler = await SetupScheduledJobs();
    }

    public static async Task Stop()
    {
        await _scheduler.Shutdown();
    }

    private static async Task<IScheduler> SetupScheduledJobs()
    {
        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
        var factory = new StdSchedulerFactory(new NameValueCollection
        {
            { "quartz.scheduler.instanceName", Assembly.GetExecutingAssembly().GetName().Name }
        });
        var scheduler = await factory.GetScheduler();
        await scheduler.ScheduleJob(JobBuilder.Create<ProcessOutboxJob>().WithIdentity("outbox").Build(), GetTrigger("outbox"));
        await scheduler.ScheduleJob(JobBuilder.Create<ProcessInboxJob>().WithIdentity("inbox").Build(), GetTrigger("inbox"));
        await scheduler.Start();
        return scheduler;
    }

    private static ITrigger GetTrigger(string name)
    {
        return TriggerBuilder.Create()
            .StartNow()
            .WithIdentity(name)
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(1)
                .RepeatForever())
            .Build();
    }
}