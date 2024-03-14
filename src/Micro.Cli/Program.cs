using Micro.Common.Application;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Users.Commands;
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
    .AddLogging(c =>
    {
        c.ClearProviders();
        c.AddSimpleConsole(o =>
        {
            o.IncludeScopes = true;
            o.SingleLine = true;
        });
        c.AddFilter("FluentMigrator", LogLevel.Warning);
        c.AddFilter("Microsoft", LogLevel.Error);
    })
    .AddSingleton<ITenantsModule, TenantsModule>()
    .AddSingleton<ITranslationModule, TranslationModule>()
    .AddInMemoryEventBus()
    .BuildServiceProvider();

var bus = services.GetRequiredService<IEventsBus>();
var logs = services.GetRequiredService<ILoggerFactory>();

await TenantsModuleStartup.Start(accessor, configuration, bus, logs, true);
await TranslationModuleStartup.Start(accessor, configuration, bus, logs, true);

var tenantsModule = services.GetRequiredService<ITenantsModule>();
var translationModule = services.GetRequiredService<ITranslationModule>();
await tenantsModule.SendCommand(new RegisterUser.Command(userId, "x", "x", $"x{Guid.NewGuid().ToString()}@example.com", "x"));
await tenantsModule.SendCommand(new UpdateUserName.Command("x", "y"));
await tenantsModule.SendCommand(new CreateOrganisation.Command(organisationId, "x"));
await tenantsModule.SendCommand(new UpdateOrganisationName.Command("y"));
await tenantsModule.SendCommand(new ProcessOutboxCommand());
await translationModule.SendCommand(new ProcessInboxCommand());