using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class ResetPasswordPage(IPage page) : PageLayout(page)
{
    private static string PasswordField => "Password";
    private static string Button => "ResetPasswordButton";

    public static async Task<ResetPasswordPage> Goto(IPage page, Guid? userId = null, Guid? token = null)
    {
        await page.GotoRelativeUrlAsync($"Auth/ResetPassword?UserId={userId}&Token={token}");
        return new ResetPasswordPage(page);
    }

    public async Task EnterPassword(string password)
    {
        await Page.GetByTestId(PasswordField).FillAsync(password);
    }

    public async Task ClickSubmit()
    {
        await Page.GetByTestId(Button).ClickAsync();
    }
}