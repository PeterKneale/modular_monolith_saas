namespace Micro.Web.AcceptanceTests.Pages.Components.ProjectSelector;

public class Component(IPage page)
{
    private const string Locator = "ProjectSelector";
       
    public async Task<bool> IsVisible() => 
        await page.GetByTestId(Locator).IsVisibleAsync();
}