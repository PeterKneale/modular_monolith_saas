using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class LoginPage(IPage page) : PageLayout(page)
{
    private static string EmailField => "Email";
    private static string PasswordField => "Password";
    private static string LoginButton => "Login";

    public static async Task<LoginPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Login");
        return new LoginPage(page);
    }

    public async Task Login(string email, string password)
    {
        await Page.GetByTestId(EmailField).FillAsync(email);
        await Page.GetByTestId(PasswordField).FillAsync(password);
        await Page.GetByTestId(LoginButton).ClickAsync();
    }
}