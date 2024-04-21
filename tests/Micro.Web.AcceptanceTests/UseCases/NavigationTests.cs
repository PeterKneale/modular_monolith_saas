using Micro.Web.AcceptanceTests.Pages;
using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;

namespace Micro.Web.AcceptanceTests.UseCases;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class NavigationTests : BaseTest
{
    [Test]
    public async Task LoginPageAllowsAnonymous() => 
        await (await LoginPage.Goto(Page)).AssertPageId();
    
    [Test]
    public async Task RegisterPageAllowsAnonymous() => 
        await (await RegisterPage.Goto(Page)).AssertPageId();
    
    [Test]
    public async Task LogoutPageAllowsAnonymousAndRedirectsToHomePage()
    {
        await LogoutPage.Goto(Page);
        var home = new HomePage(Page);
        await home.AssertPageId();
    }

    [Test]
    public async Task ForbiddenPageAllowsAnonymous() => 
        await (await ForbiddenPage.Goto(Page)).AssertPageId();
    
    [Test]
    public async Task ResetPasswordPageAllowsAnonymous() => 
        await (await ResetPasswordPage.Goto(Page)).AssertPageId();
    
    [Test]
    public async Task ForgotPasswordPagePageAllowsAnonymous() => 
        await (await ForgotPasswordPage.Goto(Page)).AssertPageId();
    
    [Test]
    public async Task VerifyPagePageAllowsAnonymous() => 
        await (await VerifyEmailPage.Goto(Page)).AssertPageId();
}