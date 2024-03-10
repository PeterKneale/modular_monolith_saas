using Micro.Common;
using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ImportingTerms
{
    private readonly ServiceFixture _service;

    public ImportingTerms(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_import_terms()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var terms = GetTerms();

        await _service.Execute(async ctx =>
        {
            await Act(ctx, terms);
            await Assert(ctx, terms);
        }, projectId: projectId);
    }

    [Fact]
    public async Task Can_import_terms_twice()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var terms = GetTerms();

        await _service.Execute(async ctx =>
        {
            await Act(ctx, terms);
            await Act(ctx, terms);
            await Assert(ctx, terms);
        }, projectId: projectId);
    }

    private static string[] GetTerms() =>
    [
        "a", "b", "c"
    ];

    private static async Task Act(IModule ctx, string[] terms)
    {
        await ctx.SendCommand(new ImportTerms.Command(terms));
    }

    private static async Task Assert(IModule ctx, string[] terms)
    {
        var results = await ctx.SendQuery(new ListTerms.Query());
        results.Select(x => x.Name).Should().BeEquivalentTo(terms);

        var count = await ctx.SendQuery(new CountTerms.Query());
        count.Should().Be(terms.Length);
    }
}