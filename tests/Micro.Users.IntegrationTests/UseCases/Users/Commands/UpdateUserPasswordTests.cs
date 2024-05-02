namespace Micro.Users.IntegrationTests.UseCases.Users.Commands;

[Collection(nameof(ServiceFixtureCollection))]
public class UpdateUserPasswordTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_login_after_changing_password()
    {
        // arrange
        var userId = Guid.NewGuid();
        var email = GenerateEmailAddress();
        var password1 = "password1";
        var password2 = "password2";

        // act
        await Service.Command(new RegisterUser.Command(userId, "x", "x", email, password1));
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        await Service.Command(new VerifyUser.Command(userId, token));
        await Service.Command(new UpdateUserPassword.Command(password1, password2), userId);

        // assert
        var results1 = await Service.Query(new CanAuthenticate.Query(email, password1));
        results1.Success.Should().BeFalse();

        var results2 = await Service.Query(new CanAuthenticate.Query(email, password2));
        results2.Success.Should().BeTrue();
        results2.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Cant_register_with_same_email_address()
    {
        // arrange
        var email = GenerateEmailAddress();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var password = "password";

        // act
        await Service.Command(new RegisterUser.Command(userId1, "x", "x", email, password));
        var act = async () => await Service.Command(new RegisterUser.Command(userId2, "x", "x", email, password));

        // assert
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

    [Fact]
    public async Task Cant_register_with_same_user_id()
    {
        // arrange
        var email1 = GenerateEmailAddress();
        var email2 = GenerateEmailAddress();
        var userId = Guid.NewGuid();
        var password = "password";

        // act
        await Service.Command(new RegisterUser.Command(userId, "x", "x", email1, password));
        var act = async () => await Service.Command(new RegisterUser.Command(userId, "x", "x", email2, password));

        // assert
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }
}