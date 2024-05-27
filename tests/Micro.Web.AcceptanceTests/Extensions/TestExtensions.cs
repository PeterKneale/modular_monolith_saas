using System.Web;

namespace Micro.Web.AcceptanceTests.Extensions;

public static class TestExtensions
{
    public static async Task<Guid> GetUserId(this IPage page, string email)
    {
        await page.GotoRelativeUrlAsync($"/Test/GetUserId?Email={HttpUtility.UrlEncode(email)}");
        return await GetGuidFromBody(page);
    }

    public static async Task<Guid> GetUserVerificationToken(this IPage page, Guid userId)
    {
        await page.GotoRelativeUrlAsync($"/Test/GetUserVerificationToken?userId={userId}");
        return await GetGuidFromBody(page);
    }

    public static async Task<Guid> GetResetPasswordToken(this IPage page, Guid userId)
    {
        await page.GotoRelativeUrlAsync($"/Test/GetPasswordResetToken?userId={userId}");
        return await GetGuidFromBody(page);
    }

    private static async Task<Guid> GetGuidFromBody(IPage page) =>
        Guid.Parse(await page.InnerTextAsync("body"));
}