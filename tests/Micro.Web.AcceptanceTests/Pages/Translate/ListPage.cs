using Micro.Web.AcceptanceTests.Pages.Auth;

namespace Micro.Web.AcceptanceTests.Pages.Translate;

public class ListPage(IPage page)
{
    private static string ListItemTestId => "ListItem";

    public static LoginPage Goto(IPage page)
    {
        page.GotoAsync(Constants.BaseUrl + "Auth/Login");
        return new LoginPage(page);
    }

    public async Task<int> Count()
    {
        var list = await page.GetByTestId(ListItemTestId).AllAsync();
        return list.Count;
    }
}