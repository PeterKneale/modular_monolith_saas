using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class GettingStatistics
{
    private readonly ServiceFixture _service;

    public GettingStatistics(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_create_term_with_multiple_translations()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();

        const string term1 = "APP_REGISTER";
        const string term2 = "APP_LOGIN";
        const string term3 = "APP_LOGOUT";
        const string text1 = "text1";
        const string text2 = "text2";
        const string text3 = "text3";

        // act
        await _service.Execute(async ctx =>
        {
            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId1, term1));
            await ctx.SendCommand(new AddTerm.Command(termId2, term2));
            await ctx.SendCommand(new AddTerm.Command(termId3, term3));

            // lang 1 is 66% translated en-AU
            await ctx.SendCommand(new AddTranslation.Command(termId1, TestLanguageCode1, text1));
            await ctx.SendCommand(new AddTranslation.Command(termId2, TestLanguageCode1, text2));

            // lang 2 is 33% translated en-UK
            await ctx.SendCommand(new AddTranslation.Command(termId1, TestLanguageCode2, text3));

            var summary = await ctx.SendQuery(new GetTranslationStatistics.Query());
            summary.TotalTerms.Should().Be(3);
            summary.Statistics.Should().BeEquivalentTo(new GetTranslationStatistics.LanguageStatistic[]
            {
                new(TestLanguageCode1, TestLanguageName1, 2, 66),
                new(TestLanguageCode2, TestLanguageName2, 1, 33)
            });
        }, projectId: projectId);
    }
}