using Micro.Translations.Application.Languages;
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

        await _service.ExecuteInContext(async ctx =>
        {
            await ctx.SendCommand(new AddLanguage.Command(languageId1, TestLanguageCode1));
        }, projectId: projectId1);

        await _service.ExecuteInContext(async ctx =>
        {
            await ctx.SendCommand(new AddLanguage.Command(languageId2, TestLanguageCode2));
        }, projectId: projectId2);

        // assert
        await _service.ExecuteInContext(async ctx =>
        {
            var results1 = await ctx.SendQuery(new ListLanguages.Query(projectId1));
            results1.Should().BeEquivalentTo(new List<ListLanguages.Result>
            {
                new(TestLanguageName1, TestLanguageCode1)
            });
        }, projectId: projectId1);

        await _service.ExecuteInContext(async ctx =>
        {
            var results2 = await ctx.SendQuery(new ListLanguages.Query(projectId2));

            results2.Should().BeEquivalentTo(new List<ListLanguages.Result>
            {
                new(TestLanguageName2, TestLanguageCode2)
            });
        }, projectId: projectId2);
    }
}