using Micro.AcceptanceTests.Pages.Layouts;

namespace Micro.AcceptanceTests.Pages.Components.AlertComponent;

public static class Extensions
{
    public static Task AssertSuccessMessageShown(this PageLayout page) =>
        page.Alert.AssertLevelIsSuccess();
}