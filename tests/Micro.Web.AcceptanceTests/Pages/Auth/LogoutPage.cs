namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class LogoutPage(IPage page)
{
    public static LogoutPage Goto(IPage page)
    {
        page.GotoAsync(Constants.BaseUrl + "Auth/Logout");
        return new LogoutPage(page);
    }
}