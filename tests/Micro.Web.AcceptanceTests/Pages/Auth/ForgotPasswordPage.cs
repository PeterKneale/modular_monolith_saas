using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class ForgotPasswordPage(IPage page) : PageLayout(page)
{
    private static string EmailField => "Email";
    private static string Button => "ForgotPasswordButton";

    public static async Task<ForgotPasswordPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/ForgotPassword");
        return new ForgotPasswordPage(page);
    }

    public async Task EnterEmail(string email)
    {
        await Page.GetByTestId(EmailField).FillAsync(email);
    }

    public async Task ClickSubmit()
    {
        await Page.GetByTestId(Button).ClickAsync();
    }
}