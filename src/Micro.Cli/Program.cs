// See https://aka.ms/new-console-template for more information

using Micro.Cli;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Context;
using Micro.Tenants;
using Micro.Translations;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Terms.Commands;
using Microsoft.Extensions.Configuration;
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
    .AddSingleton<ITenantsModule, TenantsModule>()
    .AddSingleton<ITranslationModule, TranslationModule>()
    .BuildServiceProvider();
var logs = services.GetRequiredService<ILoggerProvider>();

TenantsModuleStartup.Start(accessor, configuration, true);
TranslationModuleStartup.Start(accessor, configuration, logs, true);

await services.GetRequiredService<ITranslationModule>()
    .SendCommand(new AddTerm.Command(Guid.NewGuid(), "x"));