using Micro.Web.AcceptanceTests.Pages;
using Micro.Web.AcceptanceTests.Pages.Components.OrganisationSelector;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;

namespace Micro.Web.AcceptanceTests.UseCases.Components;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class OrganisationSelectorTests : PageTest
{
    [Test]
    public async Task A_user_with_no_organisations_on_home_page()
    {
        await Page.GivenLoggedIn();

        var home = await HomePage.Goto(Page);
        await home.AssertPageId();
        
        await home.OrganisationSelector.OrganisationSelectorShouldBeVisible();
        await home.OrganisationSelector.OrganisationShouldNotBeSelected();
        await home.OrganisationSelector.OrganisationsShouldBeEmpty();
    }
    
    [Theory]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task A_user_with_N_organisations_on_home_page_can_navigate_to_all_organisations(int count)
    {
        await Page.GivenLoggedIn();
        await Page.GivenOrganisationOwned(count);

        var home = await HomePage.Goto(Page);
        
        await home.OrganisationSelector.OrganisationSelectorShouldBeVisible();
        await home.OrganisationSelector.OrganisationShouldNotBeSelected();
        await home.OrganisationSelector.OrganisationsShouldHaveCount(count);

        for (var i = 1; i <= count; i++)
        {
            home = await HomePage.Goto(Page);
            var details = await home.OrganisationSelector.SelectOrganisationAtPosition(i);
            await details.AssertPageId();
        }
    }
    
    [Test]
    public async Task A_user_with_a_organisation_on_organisation_details_page()
    {
        // arrange
        await Page.GivenLoggedIn();
        await Page.GivenOrganisationOwned();
        
        // act
        var home = await HomePage.Goto(Page);
        var organisations = await home.OrganisationSelector.ListOrganisations();
        var organisation = organisations.Single();
        var details = await home.OrganisationSelector.SelectOrganisation(organisation);
        
        // assert   
        await details.OrganisationSelector.OrganisationSelectorShouldBeVisible();
        await details.OrganisationSelector.OrganisationShouldBeSelected();
        await details.OrganisationSelector.OrganisationsShouldBeEmpty();
    }
}