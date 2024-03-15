using Micro.AcceptanceTests.Pages.Layouts;

namespace Micro.AcceptanceTests.Pages.Auth;

public class LogoutPage(IPage page) : PageLayout(page)
{
    public static async Task<LogoutPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Logout");
        return new LogoutPage(page);
    }
}