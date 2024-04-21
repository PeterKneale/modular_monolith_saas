using FluentAssertions;
using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Layouts;
using Micro.Web.AcceptanceTests.Pages.Organisations;

namespace Micro.Web.AcceptanceTests.Pages.Components.PageId;

public static class Extensions
{
    public static async Task AssertPageId(this HomePage page) =>
        await page.AssertPageId("Home");

    public static async Task AssertPageId(this ForbiddenPage page) =>
        await page.AssertPageId("Forbidden");

    public static async Task AssertPageId(this LoginPage page) =>
        await page.AssertPageId("Login");
    
    public static async Task AssertPageId(this Micro.Web.AcceptanceTests.Pages.ApiKeys.AddPage page) =>
        await page.AssertPageId("AddApiKey");
    
    public static async Task AssertPageId(this Micro.Web.AcceptanceTests.Pages.ApiKeys.ListPage page) =>
        await page.AssertPageId("ListApiKeys");

    public static async Task AssertPageId(this RegisterPage page) =>
        await page.AssertPageId("Register");

    public static async Task AssertPageId(this VerifyEmailPage page) =>
        await page.AssertPageId("VerifyEmail");

    public static async Task AssertPageId(this ForgotPasswordPage page) =>
        await page.AssertPageId("ForgotPassword");

    public static async Task AssertPageId(this ResetPasswordPage page) =>
        await page.AssertPageId("ResetPassword");

    public static async Task AssertPageId(this OrganisationCreatePage page) =>
        await page.AssertPageId("OrganisationCreate");

    public static async Task AssertPageId(this OrganisationDetailsPage page) =>
        await page.AssertPageId("OrganisationDetails");

    private static async Task AssertPageId(this PageLayout page, string expected)
    {
        var actual = await page.PageId.GetPageId();
        actual.Should().Be(expected);
    }
}