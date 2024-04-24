using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class LogoutPage(IPage page) : PageLayout(page)
{
    public static async Task<LogoutPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Logout");
        return new LogoutPage(page);
    }
}