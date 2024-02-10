namespace Micro.Tenants.IntegrationTests.Fixtures;

public class TestContextAccessor : IContextAccessor
{
    public IUserContext? User { get; set; }
    public IOrganisationContext? Organisation { get; set; }
}