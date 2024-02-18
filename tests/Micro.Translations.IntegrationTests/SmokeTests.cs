using FluentAssertions;
using Micro.Translations.Application.Languages;
using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Translations;

namespace Micro.Translations.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class SmokeTests
{
    private readonly ServiceFixture _service;

    public SmokeTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_create_term_with_multiple_translations()
    {
        // arrange
        var organisationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var languageId1 = Guid.NewGuid();
        var languageId2 = Guid.NewGuid();
        var languageCode1 = "en-AU";
        var languageCode2 = "en-UK";
        var languageName1 = "English (Australia)";
        var languageName2 = "English (United Kingdom)";
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
            // Add languages
            await module.SendCommand(new AddLanguage.Command(languageId1, projectId, languageCode1));
            await module.SendCommand(new AddLanguage.Command(languageId2, projectId, languageCode2));

            // Add terms
            await module.SendCommand(new AddTerm.Command(termId1, projectId, term1));
            await module.SendCommand(new AddTerm.Command(termId2, projectId, term2));
            await module.SendCommand(new AddTerm.Command(termId3, projectId, term3));

            // lang 1 is 66% translated en-AU
            await module.SendCommand(new AddTranslation.Command(translationId1, termId1, languageCode1, text1));
            await module.SendCommand(new AddTranslation.Command(translationId2, termId2, languageCode1, text2));

            // lang 2 is 33% translated en-UK
            await module.SendCommand(new AddTranslation.Command(translationId3, termId1, languageCode2, text3));
        }, organisationId, userId);

        // assert
        var summary = await _service.Tenants.SendQuery(new GetTranslationStatistics.Query(projectId));
        summary.TotalTerms.Should().Be(3);
        summary.TotalTranslations.Should().Be(3);
        summary.Statistics.Should().BeEquivalentTo(new GetTranslationStatistics.Statistics[]
        {
            new(new GetTranslationStatistics.Language(languageName1, languageCode1), 66),
            new(new GetTranslationStatistics.Language(languageName2, languageCode2), 33)
        });

        var list1 = await _service.Tenants.SendQuery(new ListTranslations.Query(projectId, languageCode1));
        list1.Should().BeEquivalentTo(new ListTranslations.Result(3, 2, new ListTranslations.LanguageResult[]
        {
            new(translationId1,termId1, term1, text1),
            new(translationId2,termId2, term2, text2),
            new(null,termId3, term3, null)
        }));

        var list2 = await _service.Tenants.SendQuery(new ListTranslations.Query(projectId, languageCode2));
        list2.Should().BeEquivalentTo(new ListTranslations.Result(3, 1, new ListTranslations.LanguageResult[]
        {
            new(translationId3,termId1, term1, text3),
            new(null, termId2, term2, null),
            new(null,termId3, term3, null)
        }));
    }
}