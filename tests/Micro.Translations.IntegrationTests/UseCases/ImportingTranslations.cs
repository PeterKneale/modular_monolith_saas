using Micro.Common;
using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ImportingTranslations(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_import_translations()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var languageId = Guid.NewGuid();
        var translations = GetTranslations();

        await Service.Command(new AddLanguage.Command(languageId, TestLanguageCode1), projectId: projectId);
        await Service.Execute(async ctx =>
        {
            await Act(ctx, languageId, translations);
            await Assert(ctx, languageId, translations);
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_import_translations_twice()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var languageId = Guid.NewGuid();
        var translations = GetTranslations();
        await Service.Command(new AddLanguage.Command(languageId, TestLanguageCode1), projectId: projectId);

        await Service.Execute(async ctx =>
        {
            await Act(ctx, languageId, translations);
            await Act(ctx, languageId, translations);
            await Assert(ctx, languageId, translations);
        }, projectId: projectId);
    }

    private static Dictionary<string, string> GetTranslations() =>
        new()
        {
            { TestTerm1, "translation1" },
            { TestTerm2, "translation2" }
        };

    private static async Task Act(IModule ctx, Guid languageId, IDictionary<string, string> translations)
    {
        await ctx.SendCommand(new ImportTranslations.Command(languageId, translations));
    }

    private static async Task Assert(IModule ctx, Guid languageId, Dictionary<string, string> translations)
    {
        var languages = await ctx.SendQuery(new ListLanguagesTranslated.Query());
        languages.Select(x=>x.Code).Should().BeEquivalentTo([TestLanguageCode1]);

        // assert terms are created
        var terms = await ctx.SendQuery(new ListTerms.Query());
        terms
            .Select(x => x.Name)
            .Should()
            .BeEquivalentTo(translations.Keys);

        // assert translations are created
        var list = await ctx.SendQuery(new ListTranslations.Query(languageId));
        list.Translations.ToDictionary(x => x.TermName, x => x.TranslationText)
            .Should()
            .BeEquivalentTo(translations);

        // assert translations are created
        var count = await ctx.SendQuery(new CountTranslations.Query());
        count.Should().Be(translations.Count);
    }
}