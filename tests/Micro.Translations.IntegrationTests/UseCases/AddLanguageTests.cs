using Micro.Translations.Application.Commands;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class AddLanguageTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_add_language()
    {
        // arrange
        var languageId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var language = "en-AU";

        // act
        await Service.Command(new AddLanguage.Command(languageId, language), projectId: projectId);
    }

    [Fact]
    public async Task Cant_add_same_language_id_twice()
    {
        // arrange
        var languageId = Guid.NewGuid();
        var projectId1 = Guid.NewGuid();
        var projectId2 = Guid.NewGuid();
        var language1 = "en-AU";
        var language2 = "en-UK";

        // act
        var act = async () =>
        {
            await Service.Command(new AddLanguage.Command(languageId, language1), projectId: projectId1);
            await Service.Command(new AddLanguage.Command(languageId, language2), projectId: projectId2);
        };

        // assert
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

    [Fact]
    public async Task Cant_add_same_language_code_twice_to_one_project()
    {
        // arrange
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var language = "en-AU";

        // act
        var act = async () =>
        {
            await Service.Command(new AddLanguage.Command(languageId1, language), projectId: projectId);
            await Service.Command(new AddLanguage.Command(languageId2, language), projectId: projectId);
        };

        // assert
        await act.Should().ThrowAsync<AlreadyInUseException>();
    }
}