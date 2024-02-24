using Micro.Translations.Application.Terms;
using Micro.Translations.Application.Terms.Commands;
using Micro.Translations.Application.Terms.Queries;

namespace Micro.Translations.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class ListTermsTests
{
    private readonly ServiceFixture _service;

    public ListTermsTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_list_terms()
    {
        // arrange
        var projectId = Guid.NewGuid();
        
        var termId1 = Guid.NewGuid();
        var termId2 = Guid.NewGuid();
        var termId3 = Guid.NewGuid();

        const string term1 = "APP_REGISTER";
        const string term2 = "APP_LOGIN";
        const string term3 = "APP_LOGOUT";
        
        await _service.ExecuteInContext(async ctx =>
        {
            // act
            await ctx.SendCommand(new AddTerm.Command(termId1, term1));
            await ctx.SendCommand(new AddTerm.Command(termId2, term2));
            await ctx.SendCommand(new AddTerm.Command(termId3, term3));

            // assert
            var results = await ctx.SendQuery(new ListTerms.Query());
            results.Should().BeEquivalentTo(new List<ListTerms.Result>
            {
                new(termId1, term1),
                new(termId2, term2),
                new(termId3, term3)
            });
        }, projectId: projectId);
    }
}