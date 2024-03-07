using Micro.Translations.Application.Languages.Commands;
using Micro.Translations.Application.Terms.Commands;
using Micro.Translations.Application.Translations.Commands;
using Micro.Translations.Application.Translations.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class CountLanguageTranslationsTests
{
    private readonly ServiceFixture _service;

    public CountLanguageTranslationsTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_count_translations_per_language()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();

        // act
        await _service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId1, TestLanguageCode1));
            await ctx.SendCommand(new AddLanguage.Command(languageId2, TestLanguageCode2));

            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId1, TestTerm1));
            await ctx.SendCommand(new AddTerm.Command(termId2, TestTerm2));
            await ctx.SendCommand(new AddTerm.Command(termId3, TestTerm3));

            // lang 1
            await ctx.SendCommand(new AddTranslation.Command(termId1, languageId1, TestText1));
            await ctx.SendCommand(new AddTranslation.Command(termId2, languageId1, TestText2));

            // lang 2
            await ctx.SendCommand(new AddTranslation.Command(termId1, languageId2, TestText3));

            // assert
            var count1 = await ctx.SendQuery(new CountLanguageTranslations.Query(languageId1));
            count1.Should().Be(2);

            var count2 = await ctx.SendQuery(new CountLanguageTranslations.Query(languageId2));
            count2.Should().Be(1);
        }, projectId: projectId);
    }
}