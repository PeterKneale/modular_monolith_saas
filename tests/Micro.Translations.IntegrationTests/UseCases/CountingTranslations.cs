using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class CountingTranslations
{
    private readonly ServiceFixture _service;

    public CountingTranslations(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_count_translations_per_language()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();

        // act
        await _service.Execute(async ctx =>
        {  
            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId1, TestTerm1));
            await ctx.SendCommand(new AddTerm.Command(termId2, TestTerm2));
            await ctx.SendCommand(new AddTerm.Command(termId3, TestTerm3));

            // lang 1
            await ctx.SendCommand(new AddTranslation.Command(termId1, TestLanguageCode1, TestText1));
            await ctx.SendCommand(new AddTranslation.Command(termId2, TestLanguageCode1, TestText2));

            // lang 2
            await ctx.SendCommand(new AddTranslation.Command(termId1, TestLanguageCode2, TestText3));

            // assert
            var count1 = await ctx.SendQuery(new CountLanguageTranslations.Query(TestLanguageCode1));
            count1.Should().Be(2);

            var count2 = await ctx.SendQuery(new CountLanguageTranslations.Query(TestLanguageCode2));
            count2.Should().Be(1);
        }, projectId: projectId);
    }
}