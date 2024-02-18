namespace Micro.Web.AcceptanceTests.Pages;

public class HomePage(IPage page) : PageLayout(page)
{
    public static async Task<HomePage> Goto(IPage page)
    {
        await page.GotoAsync(BaseUrl);
        return new HomePage(page);
    }
}