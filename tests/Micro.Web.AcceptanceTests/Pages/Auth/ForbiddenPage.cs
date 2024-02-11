namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class ForbiddenPage(IPage page)
{
    public static ForbiddenPage Goto(IPage page)
    {
        page.GotoAsync(Constants.BaseUrl + "Auth/Forbidden");
        return new ForbiddenPage(page);
    }
}