using Micro.Common.Infrastructure.Context;

namespace Micro.Web.IntegrationTests.Fixtures;

public class TestContextAccessor : IContextAccessor
{
    public IUserExecutionContext? User { get; set; }
    public IOrganisationExecutionContext? Organisation { get; set; }
    public IProjectExecutionContext? Project { get; set; }
}