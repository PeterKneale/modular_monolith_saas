using Micro.Web.AcceptanceTests.Pages.Auth;

namespace Micro.Web.AcceptanceTests.UseCases.Auth;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UserVerificationTests : BaseTest
{
    [Test]
    public async Task Can_register_and_verify()
    {
        var register = await RegisterPage.Goto(Page);
        var data = TestUser.CreateValid();

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);

        var userId = await Page.GetUserId(data.Email);
        var token = await Page.GetUserVerificationToken(userId);

        var verify = await VerifyEmailPage.Goto(Page, userId, token);
        await verify.Alert.AssertSuccess("verified");
        
        await TestContext.Out.WriteLineAsync($"Registered user {data.Email} {data.Password}.");
    }

    [Test]
    public async Task UserIdMustBeSupplied()
    {
        var verify = await VerifyEmailPage.Goto(Page, null, Guid.NewGuid());
        await verify.Alert.AssertError();
    }
    
    [Test]
    public async Task TokenMustBeSupplied()
    {
        var verify = await VerifyEmailPage.Goto(Page, Guid.NewGuid(), null);
        await verify.Alert.AssertError();
    }
    
    [Test]
    public async Task Cant_verify_more_than_once()
    {
        var register = await RegisterPage.Goto(Page);
        var data = TestUser.CreateValid();

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);

        var userId = await Page.GetUserId(data.Email);
        var token = await Page.GetUserVerificationToken(userId);

        var verify = await VerifyEmailPage.Goto(Page, userId, token);
        await verify.Alert.AssertSuccess("verified");
        var verify2 = await VerifyEmailPage.Goto(Page, userId, token);
        await verify2.Alert.AssertError("This user has already been verified");
    }
    
    [Test]
    public async Task Cant_verify_with_incorrect_token()
    {
        var register = await RegisterPage.Goto(Page);
        var data = TestUser.CreateValid();

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);

        var userId = await Page.GetUserId(data.Email);
        var token = Guid.NewGuid();

        var verify = await VerifyEmailPage.Goto(Page, userId, token);
        await verify.Alert.AssertError("verification token");
    }

    [Test]
    public async Task Cant_login_until_verified()
    {
        var register = await RegisterPage.Goto(Page);
        var data = TestUser.CreateValid();

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);

        var login = await LoginPage.Goto(Page);
        await login.Login(data.Email, data.Password);
        await login.Alert.AssertError("login failed");
    }
}