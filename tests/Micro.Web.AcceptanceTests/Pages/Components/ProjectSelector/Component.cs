using Micro.Web.AcceptanceTests.Pages.Projects;

namespace Micro.Web.AcceptanceTests.Pages.Components.ProjectSelector;

public class Component(IPage page)
{
    private readonly ILocator _component = page.GetByTestId("ProjectSelector");
    private readonly ILocator _selected = page.GetByTestId("ProjectSelectorSelected");
    private readonly ILocator _options = page.GetByTestId("ProjectSelectorOption");
    private readonly ILocator _button = page.GetByTestId("ProjectSelectorActivationButton");

    public async Task<bool> IsVisible() =>
        await _component.IsVisibleAsync();

    public async Task<bool> IsProjectSelected() =>
        await _selected.IsVisibleAsync();

    public async Task<IEnumerable<string>> ListProjects() =>
        await _options.AllInnerTextsAsync();

    public async Task<string> GetSelectedProject() =>
        await _selected.InnerTextAsync();
    
    public async Task<ProjectDetailsPage> SelectTheOnlyProject()
    {
        var projects = await ListProjects();
        var project = projects.Single();
        return await SelectProject(project);
    }

    public async Task<ProjectDetailsPage> SelectProjectAtPosition(int position)
    {
        var organisations = await ListProjects();
        var index = position - 1;
        var option = organisations.ElementAtOrDefault(index);
        if (option == null)
        {
            throw new Exception($"No organisations found found at position {position}");
        }

        return await SelectProject(option);
    }

    public async Task Activate()
    {
        await _button.ClickAsync();
    }

    public async Task<ProjectDetailsPage> SelectProject(string name)
    {
        var option = await GetOption(name);
        await option.ClickAsync();
        return new ProjectDetailsPage(page);
    }

    private async Task<ILocator> GetOption(string name)
    {
        foreach (var option in await _options.AllAsync())
        {
            if (await option.InnerTextAsync() == name)
            {
                return option;
            }
        }

        throw new Exception($"Option with name {name} not found");
    }
}