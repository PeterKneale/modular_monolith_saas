using Micro.Translations.Application.Commands;
using Micro.Translations.Application.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ImportTermsTests
{
    private readonly ServiceFixture _service;

    public ImportTermsTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_import_terms()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var terms = new[]
        {
            "a", "b", "c"
        };

        await _service.Execute(async ctx =>
        {
            // act
            await ctx.SendCommand(new ImportTerms.Command(terms));

            // assert
            var results = await ctx.SendQuery(new ListTerms.Query());
            results.Select(x => x.Name).Should().BeEquivalentTo(terms);
        }, projectId: projectId);
    }
}