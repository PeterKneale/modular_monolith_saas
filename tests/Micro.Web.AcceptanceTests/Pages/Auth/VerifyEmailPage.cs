using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class VerifyEmailPage(IPage page) : PageLayout(page)
{
    public static async Task<VerifyEmailPage> Goto(IPage page, Guid? userId = null, Guid? token = null)
    {
        await page.GotoRelativeUrlAsync($"Auth/VerifyEmail?UserId={userId}&Token={token}");
        return new VerifyEmailPage(page);
    }
}