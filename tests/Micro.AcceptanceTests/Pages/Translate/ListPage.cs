using Micro.AcceptanceTests.Pages.Auth;

namespace Micro.AcceptanceTests.Pages.Translate;

public class ListPage(IPage page)
{
    private static string ListItemTestId => "ListItem";

    public static async Task<LoginPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Login");
        return new LoginPage(page);
    }

    public async Task<int> Count()
    {
        var list = await page.GetByTestId(ListItemTestId).AllAsync();
        return list.Count;
    }
}