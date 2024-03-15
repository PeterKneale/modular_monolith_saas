using Micro.AcceptanceTests.Pages.Layouts;

namespace Micro.AcceptanceTests.Pages;

public class HomePage(IPage page) : PageLayout(page)
{
    public static async Task<HomePage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("/");
        return new HomePage(page);
    }
}