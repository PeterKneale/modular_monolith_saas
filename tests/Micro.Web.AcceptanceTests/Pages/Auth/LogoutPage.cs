namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class LogoutPage(IPage page) : PageLayout(page)
{
    public static async Task<LogoutPage> Goto(IPage page)
    {
        await page.GotoAsync(BaseUrl + "Auth/Logout");
        return new LogoutPage(page);
    }
}