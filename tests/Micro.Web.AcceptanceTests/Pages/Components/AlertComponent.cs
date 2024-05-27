namespace Micro.Web.AcceptanceTests.Pages.Components;

public class AlertComponent(IPage page)
{
    private readonly ILocator _alert = page.GetByTestId("alert");

    public async Task<string> GetMessage() =>
        await _alert.InnerTextAsync();

    public async Task<bool> IsVisible() =>
        await _alert.IsVisibleAsync();

    public async Task<string> GetLevel() =>
        (await _alert.GetAttributeAsync("data-level"))!;
}