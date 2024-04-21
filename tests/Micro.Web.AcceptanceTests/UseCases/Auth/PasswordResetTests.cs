using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.Auth;

namespace Micro.Web.AcceptanceTests.UseCases.Auth;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class PasswordResetTests : BaseTest
{
    [Test]
    public async Task Can_reset_password()
    {
        var register = await RegisterPage.Goto(Page);
        var user = TestUser.CreateValid();

        await register.Register(user.FirstName, user.LastName, user.Email, user.Password);
        
        var userId = await Page.GetUserId(user.Email);
        var token = await Page.GetUserVerificationToken(userId);

        var verify = await VerifyPage.Goto(Page, userId, token);
        await verify.Alert.AssertSuccess();

        var forgot = await ForgotPasswordPage.Goto(Page);
        await forgot.EnterEmail(user.Email);
        await forgot.ClickSubmit();
        await forgot.Alert.AssertSuccess();

        var token2 = await Page.GetResetPasswordToken(userId);
        
        user.RegeneratePassword();
        
        var reset = await ResetPasswordPage.Goto(Page, userId, token2);
        await reset.EnterPassword(user.Password);
        await reset.ClickSubmit();
        await reset.Alert.AssertSuccess("Your password has been reset");
    }
    
    [Test]
    public async Task Cant_reset_password_unless_verified()
    {
        var register = await RegisterPage.Goto(Page);
        var data = TestUser.CreateValid();

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);

        var forgot = await ForgotPasswordPage.Goto(Page);
        await forgot.EnterEmail(data.Email);
        await forgot.ClickSubmit();
        await forgot.Alert.AssertError("This action must be performed on a verified user");
    }
}