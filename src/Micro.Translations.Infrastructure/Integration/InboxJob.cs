﻿using Micro.Translations.Infrastructure.Integration.Handlers;
using Quartz;

namespace Micro.Translations.Infrastructure.Integration;

[ExcludeFromCodeCoverage]
public class InboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) =>
        await CommandExecutor.SendCommand(new ProcessInboxCommand());
}