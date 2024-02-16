namespace Micro.Translations.IntegrationTests.Fixtures;

public class TestContextAccessor : IContextAccessor
{
    public IUserExecutionContext? User { get; set; }
    public IOrganisationExecutionContext? Organisation { get; set; }
    public IProjectExecutionContext? Project { get; set; }
}