using FluentAssertions;

namespace Micro.Web.AcceptanceTests.Pages.Components.OrganisationSelector;

public static class Extensions
{
    public static async Task OrganisationSelectorShouldBeVisible(this Component component) =>
        (await component.IsOrganisationSelectorVisible()).Should().BeTrue();

    public static async Task OrganisationSelectorShouldNotBeVisible(this Component component) =>
        (await component.IsOrganisationSelectorVisible()).Should().BeFalse();

    public static async Task OrganisationShouldBeSelected(this Component component) => 
        (await component.IsOrganisationSelected()).Should().BeTrue();
    
    public static async Task OrganisationsShouldBeEmpty(this Component component) => 
        (await GetOrganisationOptions(component)).Should().BeEmpty();
    
    public static async Task OrganisationsShouldHaveCount(this Component component, int count) => 
        (await GetOrganisationOptions(component)).Should().HaveCount(count);
    
    public static async Task OrganisationShouldNotBeSelected(this Component component) => 
        (await component.IsOrganisationSelected()).Should().BeFalse();

    private static async Task<bool> IsOrganisationSelected(this Component component) => 
        await component.IsOrganisationSelected();

    private static async Task<bool> IsOrganisationSelectorVisible(this Component component) =>
        await component.IsVisible();

    private static async Task<IEnumerable<string>> GetOrganisationOptions(Component component) => 
        await component.ListOrganisations();
}