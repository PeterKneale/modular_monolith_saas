using Micro.Translations.Application.Languages.Commands;
using Micro.Translations.Application.Terms.Commands;
using Micro.Translations.Application.Translations.Commands;
using Micro.Translations.Application.Translations.Queries;
using Microsoft.EntityFrameworkCore.Storage;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class TranslationTests
{
    private readonly ServiceFixture _service;

    public TranslationTests(ServiceFixture service, ITestOutputHelper outputHelper)
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

            // lang 1 is 66% translated en-AU
            await ctx.SendCommand(new AddTranslation.Command(termId1, languageId1, TestText1));
            await ctx.SendCommand(new AddTranslation.Command(termId2, languageId1, TestText2));

            // lang 2 is 33% translated en-UK
            await ctx.SendCommand(new AddTranslation.Command(termId1, languageId2, TestText3));

            var list1 = await ctx.SendQuery(new ListTranslations.Query(languageId1));
            list1.Translations.Select(x => new ValueTuple<Guid, string, string>
            {
                Item1 = x.TermId,
                Item2 = x.TermName,
                Item3 = x.TranslationText
            }).Should().BeEquivalentTo(new (Guid, string, string)[]
            {
                new(termId1, TestTerm1, TestText1),
                new(termId2, TestTerm2, TestText2),
                new(termId3, TestTerm3, null)
            });

            var list2 = await ctx.SendQuery(new ListTranslations.Query(languageId2));
            list2.Translations.Select(x => new ValueTuple<Guid, string, string>
            {
                Item1 = x.TermId,
                Item2 = x.TermName,
                Item3 = x.TranslationText
            }).Should().BeEquivalentTo(new (Guid, string, string)[]
            {
                new(termId1, TestTerm1, TestText3),
                new(termId2, TestTerm2, null),
                new(termId3, TestTerm3, null)
            });
        }, projectId: projectId);
    }
}