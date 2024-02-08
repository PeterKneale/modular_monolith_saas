using FluentAssertions;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Translations;

namespace Micro.Translations.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class SetupTests
{
    const string EN_AU = "en-AU";
    const string EN_UK = "en-UK";
    
    private readonly ServiceFixture _service;

    public SetupTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_create_term_with_multiple_translations()
    {
        // arrange
        var organisationId = Guid.NewGuid();
        var appId = Guid.NewGuid();
        var userId = Guid.NewGuid();
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
        await _service.Exec(async module =>
        {
            await module.SendCommand(new CreateTerm.Command(termId1, appId, term1));
            await module.SendCommand(new CreateTerm.Command(termId2, appId, term2));
            await module.SendCommand(new CreateTerm.Command(termId3, appId, term3));
            
            // lang 1 is 66% translated en-AU
            await module.SendCommand(new CreateTranslation.Command(translationId1, termId1, EN_AU, text1));
            await module.SendCommand(new CreateTranslation.Command(translationId2, termId2, EN_AU, text2));
            
            // lang 2 is 33% translated en-UK
            await module.SendCommand(new CreateTranslation.Command(translationId3, termId1, EN_UK, text3));
            
        }, organisationId, userId);

        // assert
        var summary = await _service.Tenants.SendQuery(new GetTranslationSummary.Query(appId));
        summary.TotalTerms.Should().Be(3);
        summary.TotalTranslations.Should().Be(3);
        summary.Languages.Select(x => x.Language).Should().BeEquivalentTo([EN_AU, EN_UK]);
        summary.Languages.Should().ContainSingle(x=>x.Language == EN_AU && x.Percentage == 66);
        summary.Languages.Should().ContainSingle(x=>x.Language == EN_UK && x.Percentage == 33);
        
        var list1 = await _service.Tenants.SendQuery(new ListTranslations.Query(appId, EN_AU));
        list1.Should().BeEquivalentTo(new ListTranslations.Result(3, 2, new ListTranslations.LanguageResult[]
        {
            new(term1, text1),
            new(term2, text2),
            new(term3, null)
        }));
        
        var list2 = await _service.Tenants.SendQuery(new ListTranslations.Query(appId, EN_UK));
        list2.Should().BeEquivalentTo(new ListTranslations.Result(3, 1, new ListTranslations.LanguageResult[]
        {
            new(term1, text3),
            new(term2, null),
            new(term3, null)
        }));
    }
}