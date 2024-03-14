using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ManagingTranslations(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_create_term_with_multiple_translations()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();

        // act
        await Service.Execute(async ctx =>
        {
            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId1, TestTerm1));
            await ctx.SendCommand(new AddTerm.Command(termId2, TestTerm2));
            await ctx.SendCommand(new AddTerm.Command(termId3, TestTerm3));

            // lang 1 is 66% translated en-AU
            await ctx.SendCommand(new AddTranslation.Command(termId1, TestLanguageCode1, TestText1));
            await ctx.SendCommand(new AddTranslation.Command(termId2, TestLanguageCode1, TestText2));

            // lang 2 is 33% translated en-UK
            await ctx.SendCommand(new AddTranslation.Command(termId1, TestLanguageCode2, TestText3));

            var list1 = await ctx.SendQuery(new ListTranslations.Query(TestLanguageCode1));
            list1.Translations.Select(x => new ValueTuple<Guid, string, string?>
            {
                Item1 = x.TermId,
                Item2 = x.TermName,
                Item3 = x.TranslationText
            }).Should().BeEquivalentTo(new (Guid, string, string?)[]
            {
                new(termId1, TestTerm1, TestText1),
                new(termId2, TestTerm2, TestText2),
                new(termId3, TestTerm3, null)
            });

            var list2 = await ctx.SendQuery(new ListTranslations.Query(TestLanguageCode2));
            list2.Translations.Select(x => new ValueTuple<Guid, string, string?>
            {
                Item1 = x.TermId,
                Item2 = x.TermName,
                Item3 = x.TranslationText
            }).Should().BeEquivalentTo(new (Guid, string, string?)[]
            {
                new(termId1, TestTerm1, TestText3),
                new(termId2, TestTerm2, null),
                new(termId3, TestTerm3, null)
            });
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_update_translation()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId = Guid.NewGuid();

        // act
        await Service.Execute(async ctx =>
        {
            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId, TestTerm1));

            // Add translation
            await ctx.SendCommand(new AddTranslation.Command(termId, TestLanguageCode1, TestText1));

            // Remove translation
            await ctx.SendCommand(new UpdateTranslation.Command(termId, TestLanguageCode1, TestText2));

            var results = await ctx.SendQuery(new ListTranslations.Query(TestLanguageCode1));
            results.TotalTranslations.Should().Be(1);
            results.Translations.Should().HaveCount(1);
            var result = results.Translations.Single();
            result.TermId.Should().Be(termId);
            result.TermName.Should().Be(TestTerm1);
            result.TranslationId.Should().NotBeNull();
            result.TranslationText.Should().Be(TestText2);
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_remove_translation()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var termId = Guid.NewGuid();

        // act
        await Service.Execute(async ctx =>
        {
            // Add terms
            await ctx.SendCommand(new AddTerm.Command(termId, TestTerm1));

            // Add translation
            await ctx.SendCommand(new AddTranslation.Command(termId, TestLanguageCode1, TestText1));

            // Remove translation
            await ctx.SendCommand(new RemoveTranslation.Command(termId, TestLanguageCode1));

            var results = await ctx.SendQuery(new ListTranslations.Query(TestLanguageCode1));
            results.TotalTranslations.Should().Be(0);
            results.Translations.Should().HaveCount(1);
            var result = results.Translations.Single();
            result.TermId.Should().Be(termId);
            result.TermName.Should().Be(TestTerm1);
            result.TranslationId.Should().BeNull();
            result.TranslationText.Should().BeNull();
        }, projectId: projectId);
    }
}