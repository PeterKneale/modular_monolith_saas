using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class ResetPasswordPage(IPage page) : PageLayout(page)
{
    private static string PasswordField => "Password";
    private static string Button => "ResetButton";
    
    public static async Task<ResetPasswordPage> Goto(IPage page, Guid userId, Guid token)
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