using Micro.Web.AcceptanceTests.Pages.Auth;

namespace Micro.Web.AcceptanceTests.UseCases.Auth;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UserRegistrationTests : BaseTest
{
    [Test]
    public async Task Can_register()
    {
        var page = await RegisterPage.Goto(Page);
        var data = TestUser.CreateValid();

        await page.Register(data.FirstName, data.LastName, data.Email, data.Password);
        await page.Alert.AssertSuccess("you have been registered");
    }

    [Test]
    public async Task Cant_register_twice_with_same_email()
    {
        var data = TestUser.CreateValid();

        var page = await RegisterPage.Goto(Page);
        await page.Register(data.FirstName, data.LastName, data.Email, data.Password);
        await page.Alert.AssertSuccess("you have been registered");

        page = await RegisterPage.Goto(Page);
        await page.Register(data.FirstName, data.LastName, data.Email, data.Password);
        await page.Alert.AssertError("already exists");
    }
}