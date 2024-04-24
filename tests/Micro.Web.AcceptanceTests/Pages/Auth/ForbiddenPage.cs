using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class ForbiddenPage(IPage page) : PageLayout(page)
{
    public static async Task<ForbiddenPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Forbidden");
        return new ForbiddenPage(page);
    }
}