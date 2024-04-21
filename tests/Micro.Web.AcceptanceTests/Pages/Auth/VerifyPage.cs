using System.Web;
using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class VerifyPage(IPage page) : PageLayout(page)
{
    public static async Task<VerifyPage> Goto(IPage page, Guid userId, Guid token)
    {
        await page.GotoRelativeUrlAsync($"Auth/Verify?UserId={userId}&Token={token}");
        return new VerifyPage(page);
    }
}