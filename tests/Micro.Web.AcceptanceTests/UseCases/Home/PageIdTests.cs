using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;
using Micro.Web.AcceptanceTests.Pages.Organisations;

namespace Micro.Web.AcceptanceTests.UseCases.Home;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class PageIdTests : BaseTest
{
    [Test]
    public async Task On_home_page()
    {
        var page = await HomePage.Goto(Page);
        await page.AssertPageId();
    }

    [Test]
    public async Task On_organisation_create_page()
    {
        await Page.GivenLoggedIn();
        await Page.GivenAnOrganisationOwned();

        var page = await OrganisationCreatePage.Goto(Page);
        await page.AssertPageId();
    }

    [Test]
    public async Task On_organisation_details_page()
    {
        await Page.GivenLoggedIn();
        await Page.GivenAnOrganisationOwned();

        var home = await HomePage.Goto(Page);
        await home.AssertPageId();

        var org = await home.OrganisationSelector.SelectOrganisationAtPosition(1);
        await org.AssertPageId();
    }
}