using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class AddTermsTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_add_term()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId = Guid.NewGuid();
        var name = "NAME";

        // act
        await Service.Command(new AddTerm.Command(termId, name), projectId: projectId);

        // assert
        var results = await Service.Query(new GetTerm.Query(termId), projectId: projectId);
        results.Name.Should().Be(name);
    }


    [Fact]
    public async Task Cant_add_same_term_id_twice()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId = Guid.NewGuid();
        var name = "NAME";

        var act = async () =>
        {
            await Service.Command(new AddTerm.Command(termId, name), projectId: projectId);
            await Service.Command(new AddTerm.Command(termId, name), projectId: projectId);
        };

        // assert
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }
}