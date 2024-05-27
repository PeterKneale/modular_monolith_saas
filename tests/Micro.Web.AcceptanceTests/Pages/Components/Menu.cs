using Micro.Web.AcceptanceTests.Pages.Organisations;
using Micro.Web.AcceptanceTests.Pages.Projects;

namespace Micro.Web.AcceptanceTests.Pages.Components;

public class Menu(IPage page)
{
    private readonly ILocator _component = page.GetByTestId("Menu");

    private readonly string _createOrganisationButton = "CreateOrganisationButton";

    private readonly string _createProjectButton = "CreateProjectButton";
    private readonly string _dropdown = "Dropdown";
    private readonly string _organisation = "Organisation";
    private readonly string _organisations = "Organisations";
    private readonly string _project = "Project";
    private readonly string _projects = "Projects";
    private readonly string _selectedOrganisation = "SelectedOrganisation";
    private readonly string _selectedProject = "SelectedProject";

    public async Task<bool> IsPresent() =>
        await _component.IsVisibleAsync();

    public async Task<bool> IsVisible() =>
        await _component.GetByTestId(_dropdown).IsVisibleAsync();

    public async Task<bool> IsProjectSelected() =>
        await _component.GetByTestId(_selectedProject).IsVisibleAsync();

    public async Task<bool> IsOrganisationSelected() =>
        await _component.GetByTestId(_selectedOrganisation).IsVisibleAsync();

    public async Task<string> GetSelectedProject() =>
        await _component.GetByTestId(_selectedProject).InnerTextAsync();

    public async Task<string> GetSelectedOrganisation() =>
        await _component.GetByTestId(_selectedOrganisation).InnerTextAsync();

    public async Task<IEnumerable<string>> GetAllProjects()
    {
        var list = await _component.GetByTestId(_projects).GetByTestId(_project).AllInnerTextsAsync();
        if (!await IsProjectSelected()) return list;
        var name = await GetSelectedProject();
        return list.Union(new[] { name });
    }

    public async Task<IEnumerable<string>> GetAllOrganisations()
    {
        var list = await _component.GetByTestId(_organisations).GetByTestId(_organisation).AllInnerTextsAsync();
        if (!await IsOrganisationSelected()) return list;
        var name = await GetSelectedOrganisation();
        return list.Union(new[] { name });
    }

    public async Task<ProjectDetailsPage> SelectTheOnlyProject()
    {
        var projects = await GetAllProjects();
        var project = projects.Single();
        return await ClickProject(project);
    }

    public async Task<OrganisationDetailsPage> SelectTheOnlyOrganisation()
    {
        var projects = await GetAllOrganisations();
        var project = projects.Single();
        return await ClickOrganisation(project);
    }

    public async Task<ProjectDetailsPage> SelectProjectAtPosition(int position)
    {
        var projects = await GetAllProjects();
        var index = position - 1;
        var option = projects.ElementAtOrDefault(index);
        if (option == null) throw new Exception($"No project found found at position {position}");

        return await ClickProject(option);
    }

    public async Task<OrganisationDetailsPage> SelectOrganisationAtPosition(int position)
    {
        var organisations = await GetAllOrganisations();
        var index = position - 1;
        var option = organisations.ElementAtOrDefault(index);
        if (option == null) throw new Exception($"No organisations found found at position {position}");

        return await ClickOrganisation(option);
    }

    public async Task Activate() => await _component.ClickAsync();

    public async Task<OrganisationCreatePage> SelectCreateOrganisation()
    {
        await _component.GetByTestId(_createOrganisationButton).ClickAsync();
        return new OrganisationCreatePage(page);
    }

    public async Task<ProjectCreatePage> SelectCreateProject()
    {
        await _component.GetByTestId(_createProjectButton).ClickAsync();
        return new ProjectCreatePage(page);
    }

    public async Task<ProjectDetailsPage> ClickProject(string name)
    {
        var option = await GetProjectOption(name);
        await option.ClickAsync();
        return new ProjectDetailsPage(page);
    }

    public async Task<OrganisationDetailsPage> ClickOrganisation(string name)
    {
        var option = await GetOrganisationOption(name);
        await option.ClickAsync();
        return new OrganisationDetailsPage(page);
    }

    private async Task<ILocator> GetProjectOption(string name)
    {
        var options = await page.GetByTestId(_projects).GetByTestId(_project).AllAsync();
        foreach (var option in options)
            if (await option.InnerTextAsync() == name)
                return option;

        throw new Exception($"Project Option with name {name} not found among {options.Count} options");
    }

    private async Task<ILocator> GetOrganisationOption(string name)
    {
        foreach (var option in await page.GetByTestId(_organisations).GetByTestId(_organisation).AllAsync())
            if (await option.InnerTextAsync() == name)
                return option;

        throw new Exception($"Organisation Option with name {name} not found");
    }
}