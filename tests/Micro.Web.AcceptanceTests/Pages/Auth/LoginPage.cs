namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class LoginPage(IPage page) : PageLayout(page)
{
    private static string EmailField => "Email";
    private static string PasswordField => "Password";
    private static string LoginButton => "Login";

    public static async Task<LoginPage> Goto(IPage page)
    {
        await page.GotoAsync(BaseUrl + "Auth/Login");
        return new LoginPage(page);
    }

    public async Task Login(string email, string password)
    {
        await page.GetByTestId(EmailField).FillAsync(email);
        await page.GetByTestId(PasswordField).FillAsync(password);
        await page.GetByTestId(LoginButton).ClickAsync();
    }
}