namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class LoginPage(IPage page)
{
    private static string EmailField => "Email";
    private static string PasswordField => "Password";
    private static string LoginButton => "Login";

    public static LoginPage Goto(IPage page)
    {
        page.GotoAsync(Constants.BaseUrl + "Auth/Login");
        return new LoginPage(page);
    }

    public async Task Login(string email, string password)
    {
        await page.GetByTestId(EmailField).FillAsync(email);
        await page.GetByTestId(PasswordField).FillAsync(password);
        await page.GetByTestId(LoginButton).ClickAsync();
    }
}