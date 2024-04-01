namespace Micro.Users.IntegrationTests.UseCases.Users;

[Collection(nameof(ServiceFixtureCollection))]
public class ResetPasswordTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Can_reset_password_using_forget_password()
    {
        // arrange
        var email = GetUniqueEmail();
        var password = "password";

        // act
        var userId = await RegisterAndVerifyUser(email, password);
        await Service.Command(new ForgotPassword.Command(email));
        var token = await Service.Query(new GetForgotPasswordToken.Query(userId));
        await Service.Command(new ResetPassword.Command(userId, token, password));

        // assert
        var result = await Service.Query(new CanAuthenticate.Query(email, password));
        result.UserId.Should().Be(userId);
    }
    
    [Fact]
    public async Task Cant_reset_password_with_invalid_token()
    {
        // arrange
        var email = GetUniqueEmail();
        var password = "password";

        // act
        var userId = await RegisterAndVerifyUser(email, password);
        await Service.Command(new ForgotPassword.Command(email));
        var act = async () => await Service.Command(new ResetPassword.Command(userId, "wrong_token", password));

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>().WithMessage("The forgot token does not match");
    }

    [Fact]
    public async Task Cant_reset_password_unless_requested()
    {
        // arrange
        var email = GetUniqueEmail();
        var password = "password";

        // act
        var userId = await RegisterAndVerifyUser(email, password);
        var act = async () => await Service.Command(new ResetPassword.Command(userId, "no_token_exists", password));

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>().WithMessage("This action must be performed on a user that has forgotten their password");
    }

    [Fact]
    public async Task Unverified_users_cannot_have_their_password_reset()
    {
        // arrange
        var email = GetUniqueEmail();
        var password = "password";

        // act
        await RegisterUser(email, password);
        var act = async () => await Service.Command(new ForgotPassword.Command(email));

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>().WithMessage("This action must be performed on a verified user");
    }
}