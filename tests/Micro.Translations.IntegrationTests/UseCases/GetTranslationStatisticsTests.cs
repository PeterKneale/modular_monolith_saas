using Micro.Translations.Application.Languages.Commands;
using Micro.Translations.Application.Terms.Commands;
using Micro.Translations.Application.Translations.Commands;
using static Micro.Translations.Application.Translations.Queries.GetTranslationStatistics;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class GetTranslationStatisticsTests
{
    private readonly ServiceFixture _service;

    public GetTranslationStatisticsTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_create_term_with_multiple_translations()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();
        var translationId1 = Guid.NewGuid();
        var translationId2 = Guid.NewGuid();
        var translationId3 = Guid.NewGuid();

        const string term1 = "APP_REGISTER";
        const string term2 = "APP_LOGIN";
        const string term3 = "APP_LOGOUT";
        const string text1 = "text1";
        const string text2 = "text2";
        const string text3 = "text3";

        // act
        await _service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId1, TestLanguageCode1));
            await ctx.SendCommand(new AddLanguage.Command(languageId2, TestLanguageCode2));

            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId1, term1));
            await ctx.SendCommand(new AddTerm.Command(termId2, term2));
            await ctx.SendCommand(new AddTerm.Command(termId3, term3));

            // lang 1 is 66% translated en-AU
            await ctx.SendCommand(new AddTranslation.Command(translationId1, termId1, languageId1, text1));
            await ctx.SendCommand(new AddTranslation.Command(translationId2, termId2, languageId1, text2));

            // lang 2 is 33% translated en-UK
            await ctx.SendCommand(new AddTranslation.Command(translationId3, termId1, languageId2, text3));

            var summary = await ctx.SendQuery(new Query());
            summary.TotalTerms.Should().Be(3);
            summary.Statistics.Should().BeEquivalentTo(new Statistic[]
            {
                new(new Language(languageId1,TestLanguageCode1), 2, 66),
                new(new Language(languageId2,TestLanguageCode2), 1, 33)
            });
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_see_languages_when_no_terms_exist()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();

        // act
        await _service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId1, TestLanguageCode1));
            await ctx.SendCommand(new AddLanguage.Command(languageId2, TestLanguageCode2));

            var summary = await ctx.SendQuery(new Query());
            summary.TotalTerms.Should().Be(0);
            summary.Statistics.Should().BeEquivalentTo(new Statistic[]
            {
                new(new Language(languageId1, TestLanguageCode1), 0, 0),
                new(new Language(languageId2, TestLanguageCode2), 0, 0)
            });
        }, projectId: projectId);
    }
}