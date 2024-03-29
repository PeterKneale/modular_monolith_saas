﻿using System.Collections.Specialized;
using System.Reflection;
using Micro.Common;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Common.Infrastructure.Jobs;
using Micro.Users.Infrastructure;
using Micro.Users.Infrastructure.Integration;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace Micro.Users;

public static class UsersModuleStartup
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
        await scheduler.ScheduleJob(JobBuilder.Create<OutboxJob>().WithIdentity("outbox").Build(), GetTrigger("outbox"));
        await scheduler.ScheduleJob(JobBuilder.Create<InboxJob>().WithIdentity("inbox").Build(), GetTrigger("inbox"));
        await scheduler.ScheduleJob(JobBuilder.Create<QueueJob>().WithIdentity("commands").Build(), GetTrigger("commands"));
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