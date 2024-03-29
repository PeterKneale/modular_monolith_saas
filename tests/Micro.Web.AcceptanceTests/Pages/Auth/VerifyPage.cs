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
    
    public static async Task<VerifyPage> Goto(IPage page, string email)
    {
        var userId = await GetUserId(page, email);
        var token = await GetUserVerificationToken(page, userId);
        return await Goto(page, userId, token);
    }

    private static async Task<Guid> GetUserId(IPage page, string email)
    {
        await page.GotoRelativeUrlAsync($"/Test/GetUserId?Email={HttpUtility.UrlEncode(email)}");
        return await GetGuidFromBody(page);
    }

    private static async Task<Guid> GetUserVerificationToken(IPage page, Guid userId)
    {
        await page.GotoRelativeUrlAsync($"/Test/GetUserVerification?userId={userId}");
        return await GetGuidFromBody(page);
    }
    
    private static async Task<Guid> GetGuidFromBody(IPage page) => 
        Guid.Parse(await page.InnerTextAsync("body"));
}