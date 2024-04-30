﻿using Micro.Users.Infrastructure.Infrastructure.Integration.Handlers;
using Quartz;

namespace Micro.Users.Infrastructure.Infrastructure.Integration;

public class OutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context) => 
        await CommandExecutor.SendCommand(new ProcessOutboxCommand());
}