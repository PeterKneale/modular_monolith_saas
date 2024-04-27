using FluentAssertions;

namespace Micro.Web.AcceptanceTests.Pages.Components;

public static class AlertAssertions
{
    public static async Task AssertSuccess(this AlertComponent alert, string? message = null)
    {
        await AssertPresent(alert);
        await AssertLevel(alert, "success");
        if (message != null)
            await AssertMessageContains(alert, message);
    }

    public static async Task AssertError(this AlertComponent alert, string? message = null)
    {
        await AssertPresent(alert);
        await AssertLevel(alert, "danger");
        if (message != null)
            await AssertMessageContains(alert, message);
    }

    private static async Task AssertPresent(this AlertComponent alert) =>
        (await alert.IsVisible()).Should().BeTrue();

    private static async Task AssertNotPresent(this AlertComponent alert) =>
        (await alert.IsVisible()).Should().BeFalse();

    private static async Task AssertMessageContains(this AlertComponent alert, string message) =>
        (await alert.GetMessage()).ToLowerInvariant()
        .Should()
        .Contain(message.ToLowerInvariant());

    private static async Task AssertLevel(this AlertComponent page, string level) =>
        (await page.GetLevel()).ToLowerInvariant().Should().Contain(level.ToLowerInvariant());
}