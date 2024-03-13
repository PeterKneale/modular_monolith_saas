﻿using Micro.Common.Application;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Tenants.Application.Users.Commands;
using Micro.Translations.Application.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var organisationId = Guid.NewGuid();
var userId = Guid.NewGuid();
var accessor = new Accessor
{
    User = new UserExecutionContext(new UserId(userId)),
    Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId)),
    Project = new ProjectExecutionContext(new ProjectId(Guid.NewGuid()))
};

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddLogging(c => { c.AddSimpleConsole(x => x.SingleLine = true); })
    .AddSingleton<ITenantsModule, TenantsModule>()
    .AddSingleton<ITranslationModule, TranslationModule>()
    .AddInMemoryEventBus()
    .BuildServiceProvider();

var bus = services.GetRequiredService<IEventsBus>();
var logs = services.GetRequiredService<ILoggerFactory>();

TenantsModuleStartup.Start(accessor, configuration, bus, logs, true);
TranslationModuleStartup.Start(accessor, configuration, bus, logs, true);

var tenantsModule = services.GetRequiredService<ITenantsModule>();
var translationModule = services.GetRequiredService<ITranslationModule>();
await tenantsModule.SendCommand(new RegisterUser.Command(Guid.NewGuid(), "x", "x", $"x{Guid.NewGuid().ToString()}@example.com", "x"));
await tenantsModule.SendCommand(new ProcessOutboxCommand());
await translationModule.SendCommand(new ProcessInboxCommand());