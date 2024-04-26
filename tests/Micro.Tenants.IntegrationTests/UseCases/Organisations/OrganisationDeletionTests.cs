using Micro.Tenants.Application.Organisations.Commands;
using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class OrganisationDeletionTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Organisation_can_be_deleted()
    {
        // arrange
        var userId =  await GivenUser();
        var organisationId = await GivenOrganisation(userId);
        
        // act
        var command = new DeleteOrganisation.Command();
        await Service.Command(command, userId, organisationId);

        // assert
        var act = async () => { await Service.Query(new GetOrganisationByContext.Query(), userId, organisationId); };
        await act.Should().ThrowAsync<NotFoundException>($"*{organisationId}");
    }
}