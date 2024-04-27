using FluentAssertions;
using Micro.Web.AcceptanceTests.Pages;
using Micro.Web.AcceptanceTests.Pages.Organisations;

namespace Micro.Web.AcceptanceTests.UseCases.Components.Menu;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class OrganisationTests : BaseTest
{
    [Test]
    public async Task An_organisation_is_shown_when_owned()
    {
        await Page.GivenLoggedIn();
        var org = await Page.GivenAnOrganisationOwned();

        // act
        var home = await HomePage.Goto(Page);
        await home.Menu.Activate();

        // assert
        (await home.Menu.GetAllOrganisations()).Should().ContainSingle(x => x == org);
    }

    [Test]
    public async Task An_organisation_can_be_clicked_on()
    {
        // arrange
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();

        // act
        var home = await HomePage.Goto(Page);
        await home.Menu.Activate();
        var details = await home.Menu.ClickOrganisation(organisation);

        // assert
        await details.AssertPageId();
    }

    [Test]
    public async Task An_organisation_is_selected_when_on_its_details_page()
    {
        // arrange
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();

        // act
        var details = await OrganisationDetailsPage.Goto(Page, organisation);
        await details.Menu.Activate();

        // assert   
        (await details.Menu.IsPresent()).Should().BeTrue();
        (await details.Menu.IsOrganisationSelected()).Should().BeTrue();
        (await details.Menu.GetAllOrganisations()).Should().ContainSingle();
        (await details.Menu.GetSelectedOrganisation()).Should().Be(organisation);
    }
    
    [Test]
    public async Task Selecting_create_organisation_navigates_to_the_organisation_creation_page()
    {
        // arrange
        await Page.GivenLoggedIn();

        // act
        var home = await HomePage.Goto(Page);
        await home.Menu.Activate();
        var create = await home.Menu.SelectCreateOrganisation();

        // assert   
        await create.AssertPageId();
    }
}