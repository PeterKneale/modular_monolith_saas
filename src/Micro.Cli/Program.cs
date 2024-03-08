// See https://aka.ms/new-console-template for more information

using Micro.Translations.Application.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var organisationId = Guid.NewGuid();
var userId = Guid.NewGuid();
var accessor = new Accessor
{
    User = new UserExecutionContext(new UserId(userId)),
    Organisation = new OrganisationExecutionContext(new OrganisationId(organisationId))
};

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddLogging(c => { c.AddSimpleConsole(x => x.SingleLine = true); })
    .AddSingleton<ITenantsModule, TenantsModule>()
    .AddSingleton<ITranslationModule, TranslationModule>()
    .BuildServiceProvider();

var logs = services.GetRequiredService<ILoggerFactory>();

TenantsModuleStartup.Start(accessor, configuration, logs, true);
TranslationModuleStartup.Start(accessor, configuration, logs, true);

await services
    .GetRequiredService<ITranslationModule>()
    .SendCommand(new AddTerm.Command(Guid.NewGuid(), "x"));