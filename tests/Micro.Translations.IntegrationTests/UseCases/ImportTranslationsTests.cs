using Micro.Translations.Application.Languages.Queries;
using Micro.Translations.Application.Terms.Commands;
using Micro.Translations.Application.Terms.Queries;
using Micro.Translations.Application.Translations.Commands;
using Micro.Translations.Application.Translations.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ImportTranslationsTests
{
    private readonly ServiceFixture _service;

    public ImportTranslationsTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Import_translations_creates_terms()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var translations = new Dictionary<string, string>
        {
            { TestTerm1, "translation1" },
            { TestTerm2, "translation2" }
        };

        await _service.Execute(async ctx =>
        {
            // act
            await ctx.SendCommand(new ImportTranslations.Command(TestLanguageCode1, translations));

            // assert langauge created
            var languages = await ctx.SendQuery(new ListLanguages.Query());
            languages.Should().BeEquivalentTo(new ListLanguages.Result[]
            {
                new(TestLanguageCode1, TestLanguageName1)
            });

            // assert terms are created
            var terms = await ctx.SendQuery(new ListTerms.Query());
            terms
                .Select(x => x.Name)
                .Should()
                .BeEquivalentTo(translations.Keys);

            // assert translations are created
            var results = await ctx.SendQuery(new ListTranslations.Query(TestLanguageCode1));
            results.Translations.ToDictionary(x => x.TermName, x => x.TranslationText)
                .Should()
                .BeEquivalentTo(translations);
        }, projectId: projectId);
    }
}