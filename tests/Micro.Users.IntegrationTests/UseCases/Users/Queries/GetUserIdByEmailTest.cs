namespace Micro.Users.IntegrationTests.UseCases.Users.Queries;

[Collection(nameof(ServiceFixtureCollection))]
public class GetUserIdByEmailTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Id_can_be_retrieved()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";
        var userId = await GivenVerifiedUser(email, password);

        // act
        var result = await Service.Query(new GetUserIdByEmail.Query(email));

        // assert
        result.Should().Be(userId);
    }

    [Fact]
    public async Task Id_can_be_retrieved_when_email_case_is_changed()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";
        var userId = await GivenVerifiedUser(email, password);

        // act
        var uppercaseEmail = email.ToUpperInvariant();
        var result = await Service.Query(new GetUserIdByEmail.Query(uppercaseEmail));

        // assert
        result.Should().Be(userId);
    }

    [Fact]
    public async Task Throws_not_found_exception_when_not_found()
    {
        // arrange
        var email = GenerateEmailAddress();

        // act
        var act = async () => { await Service.Query(new GetUserIdByEmail.Query(email)); };

        // assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}