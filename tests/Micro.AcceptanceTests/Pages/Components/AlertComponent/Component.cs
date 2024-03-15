using FluentAssertions;

namespace Micro.AcceptanceTests.Pages.Components.AlertComponent;

public class Component(IPage page)
{
    private readonly ILocator _alert = page.GetByTestId("alert");

    // not sure about this other style of having assertions in the page
    // or in the tests
    public async Task AssertVisible() =>
        await Assertions
            .Expect(_alert)
            .ToBeVisibleAsync();
    
    public async Task AssertNotVisible() =>
        await Assertions
            .Expect(_alert)
            .Not
            .ToBeVisibleAsync();

    public async Task AssertLevelIsSuccess() =>
        await AssertLevelIs("success");

    public async Task AssertLevelIs(string level) =>
        await Assertions
            .Expect(_alert)
            .ToHaveClassAsync($"alert alert-{level}");
    
    public async Task AssertMessageContains(string message)
    {
        var content = await _alert.InnerTextAsync();
        content.ToLowerInvariant().Should().Contain(message.ToLowerInvariant());
    }
}