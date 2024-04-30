using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class CountingTranslations(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_count_translations_when_no_terms_exist()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var languageId = Guid.NewGuid();
        
        // act
        await Service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId, "en-AU"));
            
            // assert
            var count = await ctx.SendQuery(new CountLanguageTranslations.Query(languageId));
            count.Should().Be(0);
        }, projectId: projectId);
    }
    
    [Fact]
    public async Task Can_count_translations_when_terms_exist_but_no_translations_exist()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId = Guid.NewGuid();
        var languageId = Guid.NewGuid();
        
        // act
        await Service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId, "en-AU"));
            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId, TestTerm1));
            
            // assert
            var count = await ctx.SendQuery(new CountLanguageTranslations.Query(languageId));
            count.Should().Be(0);
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_count_translations_per_language()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();

        // act
        await Service.Execute(async ctx =>
        {
            // Add languages
            await ctx.SendCommand(new AddLanguage.Command(languageId1, "en-AU"));
            await ctx.SendCommand(new AddLanguage.Command(languageId2, "en-GB"));
            
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