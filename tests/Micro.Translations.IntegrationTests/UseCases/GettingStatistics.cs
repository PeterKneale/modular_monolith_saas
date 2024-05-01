using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class GettingStatistics(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_create_term_with_multiple_translations()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();

        const string term1 = "APP_REGISTER";
        const string term2 = "APP_LOGIN";
        const string term3 = "APP_LOGOUT";
        const string text1 = "text1";
        const string text2 = "text2";
        const string text3 = "text3";

        // act
        await Service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId1, TestLanguageCode1));
            await ctx.SendCommand(new AddLanguage.Command(languageId2, TestLanguageCode2));

            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId1, term1));
            await ctx.SendCommand(new AddTerm.Command(termId2, term2));
            await ctx.SendCommand(new AddTerm.Command(termId3, term3));

            // lang 1 is 66% translated en-AU
            await ctx.SendCommand(new AddTranslation.Command(termId1, languageId1, text1));
            await ctx.SendCommand(new AddTranslation.Command(termId2, languageId1, text2));

            // lang 2 is 33% translated en-UK
            await ctx.SendCommand(new AddTranslation.Command(termId1, languageId2, text3));

            var summary = await ctx.SendQuery(new ListLanguageStatistics.Query());
            summary.TotalTerms.Should().Be(3);
            summary.Statistics.Select(x => x.Language.Name)
                .Should()
                .BeEquivalentTo([TestLanguageName1, TestLanguageName2]);
            summary.Statistics.Select(x => x.Language.Code)
                .Should()
                .BeEquivalentTo([TestLanguageCode1, TestLanguageCode2]);
            summary.Statistics.Select(x => x.Count)
                .Should()
                .BeEquivalentTo([2, 1]);
            summary.Statistics.Select(x => x.Percentage)
                .Should()
                .BeEquivalentTo([66, 33]);
        }, projectId: projectId);
    }
}