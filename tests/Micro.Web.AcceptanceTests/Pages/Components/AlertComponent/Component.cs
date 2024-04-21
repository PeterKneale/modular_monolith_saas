namespace Micro.Web.AcceptanceTests.Pages.Components.AlertComponent;

public class Component(IPage page)
{
    private readonly ILocator _alert = page.GetByTestId("alert");

    public async Task AssertVisible() =>
        await Assertions
            .Expect(_alert)
            .ToBeVisibleAsync();

    public async Task AssertNotVisible() =>
        await Assertions
            .Expect(_alert)
            .Not
            .ToBeVisibleAsync();
    
    public async Task AssertSuccess(string? message = null)
    {
        await AssertVisible();
        await AssertLevel("success");
        if (message != null)
            await AssertMessageContains(message);
    }

    public async Task AssertError(string? message = null)
    {
        await AssertVisible();
        await AssertLevel("danger");
        if (message != null)
            await AssertMessageContains(message);
    }

    private async Task AssertMessageContains(string message) =>
        await Assertions
            .Expect(_alert)
            .ToContainTextAsync(message, new LocatorAssertionsToContainTextOptions
            {
                UseInnerText = true,
                IgnoreCase = true
            });


    private async Task AssertLevel(string level) =>
        await Assertions
            .Expect(_alert)
            .ToHaveClassAsync($"alert alert-{level}");
}