// See https://aka.ms/new-console-template for more information

using Micro.Common.Application;
using Micro.Common.Domain;
using Micro.Common.Infrastructure.Context;
using Micro.Tenants;
using Micro.Translations;
using Micro.Translations.Application.Terms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var organisationId = Guid.NewGuid();
var userId = Guid.NewGuid();
var accessor = new Accessor
{
    CurrentContext = new CurrentContext(new OrganisationId(organisationId), new UserId(userId))
};

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddSingleton<ITenantsModule, TenantsModule>()
    .AddSingleton<ITranslationModule, TranslationModule>()
    .BuildServiceProvider();

TenantsModuleStartup.Start(accessor, configuration, true);
TranslationModuleStartup.Start(accessor, configuration, true);

await services.GetRequiredService<ITranslationModule>()
    .SendCommand(new CreateTerm.Command(Guid.NewGuid(), Guid.NewGuid(), "x"));

public class Accessor : IContextAccessor
{
    public ICurrentContext? CurrentContext { get; set; }
}