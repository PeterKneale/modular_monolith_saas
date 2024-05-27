using FluentAssertions;
using Micro.Web.AcceptanceTests.Pages.Organisations;
using Micro.Web.AcceptanceTests.Pages.Projects;

namespace Micro.Web.AcceptanceTests.UseCases.Components.Menu;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ProjectTests : BaseTest
{
    [Test]
    public async Task An_project_is_shown()
    {
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        var project = await Page.GivenAProjectOwned(organisation);

        // act
        var home = await OrganisationDetailsPage.Goto(Page, organisation);
        await home.Menu.Activate();

        // assert
        (await home.Menu.GetAllProjects()).Should().ContainSingle(x => x == project);
    }

    [Test]
    public async Task A_project_can_be_clicked_on()
    {
        // arrange
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        var project = await Page.GivenAProjectOwned(organisation);

        // act
        var organisationPage = await OrganisationDetailsPage.Goto(Page, organisation);
        await organisationPage.Menu.Activate();
        var projectPage = await organisationPage.Menu.ClickProject(project);

        // assert
        await projectPage.AssertPageId();
    }

    [Test]
    public async Task A_project_is_selected_when_on_its_details_page()
    {
        // arrange
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        var project = await Page.GivenAProjectOwned(organisation);

        // act
        var details = await ProjectDetailsPage.Goto(Page, organisation, project);
        await details.Menu.Activate();

        // assert   
        (await details.Menu.IsProjectSelected()).Should().BeTrue();
        (await details.Menu.GetAllOrganisations()).Should().ContainSingle();
        (await details.Menu.GetSelectedOrganisation()).Should().Be(organisation);
        (await details.Menu.GetSelectedProject()).Should().Be(project);
    }

    [Test]
    public async Task Selecting_create_project_navigates_to_the_project_creation_page()
    {
        // arrange
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();

        // act
        var organisationPage = await OrganisationDetailsPage.Goto(Page, organisation);
        await organisationPage.Menu.Activate();
        var create = await organisationPage.Menu.SelectCreateProject();

        // assert   
        await create.AssertPageId();
    }
}