namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class ForbiddenPage(IPage page) : PageLayout(page)
{
    public static async Task<ForbiddenPage> Goto(IPage page)
    {
        await page.GotoAsync(BaseUrl + "Auth/Forbidden");
        return new ForbiddenPage(page);
    }
}