using Micro.Translations.Application.Languages.Commands;
using Micro.Translations.Application.Languages.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class LanguageTests
{
    private readonly ServiceFixture _service;

    public LanguageTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        _service = service;
        service.OutputHelper = outputHelper;
    }

    [Fact]
    public async Task LanguagesCanBeListedByProject()
    {
        var projectId1 = Guid.NewGuid();
        var projectId2 = Guid.NewGuid();

        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();

        await _service.Command(new AddLanguage.Command(languageId1, TestLanguageCode1), projectId: projectId1);

        await _service.Command(new AddLanguage.Command(languageId2, TestLanguageCode2), projectId: projectId2);

        // assert
        var results1 = await _service.Query(new ListLanguages.Query(), projectId: projectId1);
        results1.Select(x => x.Name).Should().BeEquivalentTo([
            TestLanguageName1
        ]);

        var results2 = await _service.Query(new ListLanguages.Query(), projectId: projectId2);
        results2.Select(x => x.Name).Should().BeEquivalentTo([
            TestLanguageName2
        ]);
    }
}