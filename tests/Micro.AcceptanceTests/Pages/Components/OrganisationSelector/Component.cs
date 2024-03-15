using Micro.AcceptanceTests.Pages.Organisations;

namespace Micro.AcceptanceTests.Pages.Components.OrganisationSelector;

public class Component(IPage page)
{
    private readonly ILocator _component = page.GetByTestId("OrganisationSelector");
    private readonly ILocator _selected = page.GetByTestId("OrganisationSelected");
    private readonly ILocator _options = page.GetByTestId("OrganisationOption");
    private readonly ILocator _button = page.GetByTestId("OrganisationSelectorActivationButton");

    public async Task<bool> IsVisible() =>
        await _component.IsVisibleAsync();

    public async Task<bool> IsOrganisationSelected() =>
        await _selected.IsVisibleAsync();

    public async Task<IEnumerable<string>> ListOrganisations() =>
        await _options.AllInnerTextsAsync();

    public async Task<object> GetSelectedOrganisation() => 
        await _selected.InnerTextAsync();

    public async Task<OrganisationDetailsPage> SelectTheOnlyOrganisation()
    {
        var organisations = await ListOrganisations();
        var organisation = organisations.Single();
        return await SelectOrganisation(organisation);
    }
    
    public async Task<OrganisationDetailsPage> SelectOrganisationAtPosition(int position)
    {
        var organisations = await ListOrganisations();
        var index = position - 1;
        var option = organisations.ElementAtOrDefault(index);
        if (option == null)
        {
            throw new Exception($"No organisations found found at position {position}");
        }
        return await SelectOrganisation(option);
    }

    public async Task<OrganisationDetailsPage> SelectOrganisation(string name)
    {
        await _button.ClickAsync();
        var option = await GetOption(name);
        await option.ClickAsync();
        return new OrganisationDetailsPage(page);
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