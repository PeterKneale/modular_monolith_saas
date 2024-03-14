using Micro.Tenants.Application.Organisations.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Organisations;

[Collection(nameof(ServiceFixtureCollection))]
public class DeletionTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Organisation_can_be_deleted()
    {
        // arrange
        var userId = await RegisterAndVerifyUser();
        var organisationId = await CreateOrganisation(userId);
        
        // act
        await DeleteOrganisation(userId, organisationId);

        // assert
        var act = async () => { await Service.Query(new GetOrganisation.Query(), userId, organisationId); };
        await act.Should().ThrowAsync<NotFoundException>($"*{organisationId}");
    }
}