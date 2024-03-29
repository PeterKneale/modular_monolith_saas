using FluentAssertions;
using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;
using Micro.Web.AcceptanceTests.Pages.Projects;

namespace Micro.Web.AcceptanceTests.UseCases.Components;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ProjectSelectorTests: BaseTest
{
    [Test]
    public async Task A_user_is_on_home_page_an_owns_no_organisations()
    {
        await Page.GivenLoggedIn();

        var home = await HomePage.Goto(Page);
        await home.AssertPageId();

        (await home.ProjectSelector.IsVisible()).Should().BeFalse();
    }

    [Test]
    public async Task A_user_is_on_home_page_and_owns_an_organisation()
    {
        await Page.GivenLoggedIn();
        await Page.GivenAnOrganisationOwned();

        var home = await HomePage.Goto(Page);

        (await home.ProjectSelector.IsVisible()).Should().BeFalse();
    }

    [Test]
    public async Task A_user_selects_an_organisation()
    {
        await Page.GivenLoggedIn();
        await Page.GivenAnOrganisationOwned();

        var home = await HomePage.Goto(Page);
        var details = await home.OrganisationSelector.SelectTheOnlyOrganisation();

        await details.AssertPageId();
        (await home.ProjectSelector.IsVisible()).Should().BeTrue();
        (await home.ProjectSelector.ListProjects()).Should().BeEmpty();
    }

    [Test]
    public async Task A_user_on_the_create_project_page()
    {
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();

        var createPage = await ProjectCreatePage.Goto(Page, organisation);

        (await createPage.ProjectSelector.IsVisible()).Should().BeTrue();
        (await createPage.ProjectSelector.ListProjects()).Should().BeEmpty();
    }

    [Test]
    public async Task A_user_on_the_create_project_page_a_single_project()
    {
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        await Page.GivenAProjectOwned(organisation);

        var createPage = await ProjectCreatePage.Goto(Page, organisation);

        (await createPage.ProjectSelector.IsVisible()).Should().BeTrue();
        (await createPage.ProjectSelector.ListProjects()).Should().HaveCount(1);
    }

    [Test]
    public async Task A_user_on_the_create_project_page_multiple_projects()
    {
        const int count = 3;
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        await Page.GivenProjectsOwned(organisation, count);

        var createPage = await ProjectCreatePage.Goto(Page, organisation);

        (await createPage.ProjectSelector.IsVisible()).Should().BeTrue();
        (await createPage.ProjectSelector.ListProjects()).Should().HaveCount(count);
    }
    
    [Test]
    public async Task A_user_on_the_project_details_page()
    {
        await Page.GivenLoggedIn();
        var organisation = await Page.GivenAnOrganisationOwned();
        var project = await Page.GivenAProjectOwned(organisation);

        var detailsPage = await ProjectDetailsPage.Goto(Page, organisation, project);

        (await detailsPage.ProjectSelector.IsVisible()).Should().BeTrue();
        (await detailsPage.ProjectSelector.ListProjects()).Should().HaveCount(0);
        (await detailsPage.ProjectSelector.IsProjectSelected()).Should().BeTrue();
        (await detailsPage.ProjectSelector.GetSelectedProject()).Should().Be(project);
    }
}