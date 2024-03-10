using Micro.Common;
using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ImportingTranslations
{
    private readonly ServiceFixture _service;

    public ImportingTranslations(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_import_translations()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var translations = GetTranslations();

        await _service.Execute(async ctx =>
        {
            await Act(ctx, translations);
            await Assert(ctx, translations);
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_import_translations_twice()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var translations = GetTranslations();

        await _service.Execute(async ctx =>
        {
            await Act(ctx, translations);
            await Act(ctx, translations);
            await Assert(ctx, translations);
        }, projectId: projectId);
    }


    private static Dictionary<string, string> GetTranslations() =>
        new()
        {
            { TestTerm1, "translation1" },
            { TestTerm2, "translation2" }
        };

    private static async Task Act(IModule ctx, IDictionary<string, string> translations) =>
        await ctx.SendCommand(new ImportTranslations.Command(TestLanguageCode1, translations));

    private static async Task Assert(IModule ctx, Dictionary<string, string> translations)
    {
        var languages = await ctx.SendQuery(new ListLanguagesTranslated.Query());
        languages.Should().BeEquivalentTo(new ListLanguagesTranslated.Result[]
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
        var list = await ctx.SendQuery(new ListTranslations.Query(TestLanguageCode1));
        list.Translations.ToDictionary(x => x.TermName, x => x.TranslationText)
            .Should()
            .BeEquivalentTo(translations);

        // assert translations are created
        var count = await ctx.SendQuery(new CountProjectTranslations.Query());
        count.Should().Be(translations.Count);
    }
}