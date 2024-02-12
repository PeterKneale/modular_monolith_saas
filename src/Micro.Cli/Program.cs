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
    User = new UserContext(new UserId(userId)),
    Organisation = new OrganisationContext(new OrganisationId(organisationId))
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
    .SendCommand(new AddTerm.Command(Guid.NewGuid(), Guid.NewGuid(), "x"));

public class Accessor : IContextAccessor
{
    public IUserContext? User { get; set; }
    public IOrganisationContext? Organisation { get; set; }
    public IProjectContext? Project { get; set; }
}