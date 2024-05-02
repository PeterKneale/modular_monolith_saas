namespace Micro.Users.IntegrationTests.UseCases.Users.Commands;

[Collection(nameof(ServiceFixtureCollection))]
public class UpdateUserNameTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Current_user_can_be_retrieved()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";
        var userId = await GivenVerifiedUser(email, password);
        var firstName = "John";
        var lastName = "Doe";

        // act
        await Service.Command(new UpdateUserName.Command(firstName, lastName), userId);

        // assert
        var result = await Service.Query(new GetCurrentUser.Query(), userId);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
    }
}