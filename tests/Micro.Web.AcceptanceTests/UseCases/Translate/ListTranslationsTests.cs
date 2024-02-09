using FluentAssertions;
using Micro.Web.AcceptanceTests.Pages.Translate;

namespace Micro.Web.AcceptanceTests.UseCases.Translate;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ListTranslationsTests : PageTest
{
    [Test]
    public async Task Can_list_translations()
    {
        var page = new ListPage(Page);
        var count = await page.Count();
        count.Should().Be(0);
    }
}