using Micro.Web.AcceptanceTests.Pages.Auth;

namespace Micro.Web.AcceptanceTests.UseCases;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SmokeTests : BaseTest
{
    [Test]
    public async Task Can_register()
    {
        var registerPage = await RegisterPage.Goto(Page);
        var user = TestUser.CreateValid();

        await registerPage.Register(user.FirstName, user.LastName, user.Email, user.Password);
        await registerPage.Alert.AssertSuccess();
    }
}