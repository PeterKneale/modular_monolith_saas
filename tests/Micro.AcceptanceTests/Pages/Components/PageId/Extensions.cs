using FluentAssertions;
using Micro.AcceptanceTests.Pages.Layouts;
using Micro.AcceptanceTests.Pages.Organisations;

namespace Micro.AcceptanceTests.Pages.Components.PageId;

public static class Extensions
{
    public static async Task AssertPageId(this HomePage page) =>
        await Assert(page, "HomePage");
    
    public static async Task AssertPageId(this OrganisationCreatePage page) =>
        await Assert(page, "OrganisationCreate");
    
    public static async Task AssertPageId(this OrganisationDetailsPage page) =>
        await Assert(page, "OrganisationDetails");
    
    private static async Task Assert(PageLayout page, string expected)
    {
        var actual = await page.PageId.GetPageId();
        actual.Should().Be(expected);
    }
}