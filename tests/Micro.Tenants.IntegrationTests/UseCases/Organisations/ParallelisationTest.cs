using Micro.Tenants.Application.Organisations.Commands;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class ParallelisationTest(ServiceFixture service, ITestOutputHelper outputHelper)
    : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Many_can_be_created_at_once()
    {

        // arrange
        var userId = await GivenUser();
        var commands = Enumerable.Range(0, 10)
            .Select(_ => new CreateOrganisation.Command(Guid.NewGuid(), Guid.NewGuid().ToString()[..10]))
            .ToList();

        // act

        var tasks = commands.Select(command => Service.Command(command, userId));
        await Task.WhenAll(tasks);

        // assert
    }
}