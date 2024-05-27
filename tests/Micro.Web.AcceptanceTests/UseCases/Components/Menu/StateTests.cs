using FluentAssertions;
using Micro.Web.AcceptanceTests.Pages;

namespace Micro.Web.AcceptanceTests.UseCases.Components.Menu;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class StateTests : BaseTest
{
    [SetUp]
    public async Task SetUp()
    {
        await Page.GivenLoggedIn();
        _home = await HomePage.Goto(Page);
    }

    private HomePage _home = null!;

    [Test]
    public async Task The_menu_is_present() => (await _home.Menu.IsPresent()).Should().BeTrue();

    [Test]
    public async Task The_menu_is_not_visible() => (await _home.Menu.IsVisible()).Should().BeFalse();

    [Test]
    public async Task The_menu_is_visible_when_clicked()
    {
        // act
        await _home.Menu.Activate();

        // assert
        (await _home.Menu.IsVisible()).Should().BeTrue();
    }

    [Test]
    public async Task The_organisation_list_is_empty_when_no_organisations_are_owned()
    {
        // act
        await _home.Menu.Activate();

        // assert
        (await _home.Menu.GetAllOrganisations()).Should().BeEmpty();
    }

    [Test]
    public async Task The_project_list_is_empty_when_no_organisations_are_owned()
    {
        // act
        await _home.Menu.Activate();

        // assert
        (await _home.Menu.GetAllProjects()).Should().BeEmpty();
    }
}