namespace Micro.Users.IntegrationTests.UseCases.Users.Commands;

[Collection(nameof(ServiceFixtureCollection))]
public class VerifyUserTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Verification_token_must_be_correct()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";

        // act
        var userId = await GivenRegisteredUser(email, password);
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        var act = async () => { await Service.Command(new VerifyUser.Command(userId, token + "x")); };

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("*token*");
    }

    [Fact]
    public async Task Cant_verify_with_wrong_email()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";

        // act
        var userId = await GivenRegisteredUser(email, password);
        var verification = await Service.Query(new GetUserVerificationToken.Query(userId));
        var act = async () => await Service.Command(new VerifyUser.Command(Guid.NewGuid(), verification));

        // assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Cant_verify_with_wrong_verification()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";

        // act
        var userId = await GivenRegisteredUser(email, password);
        var act = async () => await Service.Command(new VerifyUser.Command(userId, "wrong"));

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>().WithMessage("The verification token does not match");
    }

    [Fact]
    public async Task Can_not_verify_multiple_times()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";

        // act
        var userId = await GivenRegisteredUser(email, password);
        var verification = await Service.Query(new GetUserVerificationToken.Query(userId));
        await Service.Command(new VerifyUser.Command(userId, verification));
        var act = async () => await Service.Command(new VerifyUser.Command(userId, verification));

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>().WithMessage("This user has already been verified");
    }
}