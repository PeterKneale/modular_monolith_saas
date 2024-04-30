using Micro.Common.Application;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Infrastructure;
using Micro.Translations.Infrastructure;
using Micro.Users.Application.Users.Commands;
using Micro.Users.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExecutionContext = Micro.Common.Infrastructure.Context.ExecutionContext;

var organisationId = Guid.NewGuid();
var userId = Guid.NewGuid();
var projectId = Guid.NewGuid();
var accessor = new Accessor
{
    ExecutionContext = ExecutionContext.Create(userId, organisationId, projectId)
};

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddLogging(builder => builder.AddSimpleConsole(x=>x.SingleLine = true))
    .AddSingleton<IUsersModule, UsersModule>()
    .AddSingleton<ITenantsModule, TenantsModule>()
    .AddSingleton<ITranslationModule, TranslationModule>()
    .AddInMemoryEventBus()
    .BuildServiceProvider();

var bus = services.GetRequiredService<IEventsBus>();
var logs = services.GetRequiredService<ILoggerFactory>();

await UsersModuleStartup.Start(accessor, configuration, bus, logs, true);
await TenantsModuleStartup.Start(accessor, configuration, bus, logs, true);
await TranslationModuleStartup.Start(accessor, configuration, bus, logs, true);

var usersModule = services.GetRequiredService<IUsersModule>();
var tenantsModule = services.GetRequiredService<ITenantsModule>();
var translationModule = services.GetRequiredService<ITranslationModule>();
await usersModule.SendCommand(new RegisterUser.Command(userId, "x", "x", $"x{Guid.NewGuid().ToString()}@example.com", "x"));
await usersModule.SendCommand(new UpdateUserName.Command("x", "y"));
await usersModule.SendCommand(new ProcessOutboxCommand());

await tenantsModule.SendCommand(new ProcessInboxCommand());
await translationModule.SendCommand(new ProcessInboxCommand());

await tenantsModule.SendCommand(new CreateOrganisation.Command(organisationId, "x"));
await tenantsModule.SendCommand(new UpdateOrganisationName.Command("y"));
await tenantsModule.SendCommand(new ProcessOutboxCommand());

await translationModule.SendCommand(new ProcessInboxCommand());
await Task.Delay(TimeSpan.FromSeconds(10));