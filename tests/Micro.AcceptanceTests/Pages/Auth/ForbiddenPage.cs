using Micro.AcceptanceTests.Pages.Layouts;

namespace Micro.AcceptanceTests.Pages.Auth;

public class ForbiddenPage(IPage page) : PageLayout(page)
{
    public static async Task<ForbiddenPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Forbidden");
        return new ForbiddenPage(page);
    }
}