using FluentAssertions;

namespace Micro.Web.AcceptanceTests.Pages.Components.ProjectSelector;

public static class Extensions
{
    public static async Task ProjectSelectorShouldBeVisible(this PageLayout page, string because) =>
        (await page.IsProjectSelectorVisible()).Should().BeTrue();

    public static async Task ProjectSelectorShouldNotBeVisible(this PageLayout page, string? because = null) =>
        (await page.IsProjectSelectorVisible()).Should().BeFalse(because);
    
    private static async Task<bool> IsProjectSelectorVisible(this PageLayout page) =>
        await page.ProjectSelector.IsVisible();
}