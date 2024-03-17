using FluentAssertions;
using Micro.AcceptanceTests.Pages;
using Micro.AcceptanceTests.Pages.Components.PageId;
using Micro.AcceptanceTests.Pages.Organisations;

namespace Micro.AcceptanceTests.UseCases.Components;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class OrganisationSelectorTests: BaseTest
{
    [Test]
    public async Task A_user_with_no_organisations_on_home_page()
    {
        await Page.GivenLoggedIn();

        var home = await HomePage.Goto(Page);
        await home.AssertPageId();
        
        (await home.OrganisationSelector.IsVisible()).Should().BeTrue();
        (await home.OrganisationSelector.IsOrganisationSelected()).Should().BeFalse();
        (await home.OrganisationSelector.ListOrganisations()).Should().BeEmpty();
    }
    
    [Theory]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task A_user_with_multiple_organisations_can_navigate_to_all_organisations_from_home_page(int count)
    {
        await Page.GivenLoggedIn();
        var organisations = await Page.GivenANumberOfOrganisationOwned(count);

        var home = await HomePage.Goto(Page);
        
        (await home.OrganisationSelector.IsVisible()).Should().BeTrue();
        (await home.OrganisationSelector.IsOrganisationSelected()).Should().BeFalse();
        (await home.OrganisationSelector.ListOrganisations()).Should().HaveCount(count);

        foreach (var organisation in organisations)
        {
            home = await HomePage.Goto(Page);
            var details = await home.OrganisationSelector.SelectOrganisation(organisation);
            await details.AssertPageId();
            (await details.OrganisationSelector.GetSelectedOrganisation()).Should().Be(organisation);
        }
    }
    
    [Test]
    public async Task A_user_with_a_organisation_on_organisation_details_page()
    {
        // arrange
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        
        // act
        var details = await OrganisationDetailsPage.Goto(Page, organisation);
        
        // assert   
        (await details.OrganisationSelector.IsVisible()).Should().BeTrue();
        (await details.OrganisationSelector.IsOrganisationSelected()).Should().BeTrue();
        (await details.OrganisationSelector.ListOrganisations()).Should().BeEmpty();
        (await details.OrganisationSelector.GetSelectedOrganisation()).Should().Be(organisation);
    }
}