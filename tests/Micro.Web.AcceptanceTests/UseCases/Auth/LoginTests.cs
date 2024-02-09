using Micro.Web.AcceptanceTests.Pages.Auth;

namespace Micro.Web.AcceptanceTests.UseCases.Auth;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class LoginTests : PageTest
{
    [Test]
    public async Task Can_login()
    {
        var loginPage = LoginPage.Goto(Page);
        await loginPage.Login("test@example.com", "password");
    }
}