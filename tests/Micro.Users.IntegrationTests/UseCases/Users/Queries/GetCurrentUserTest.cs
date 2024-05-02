namespace Micro.Users.IntegrationTests.UseCases.Users.Queries;

[Collection(nameof(ServiceFixtureCollection))]
public class GetCurrentUserTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Current_user_can_be_retrieved()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";
        var userId = await GivenVerifiedUser(email, password);

        // act
        var result = await Service.Query(new GetCurrentUser.Query(), userId);

        // assert
        result.Id.Should().Be(userId);
    }

    [Fact]
    public async Task Throw_context_exception_when_no_current_user()
    {
        // arrange
        var email = GenerateEmailAddress();
        var password = "password";
        var userId = await GivenVerifiedUser(email, password);

        // act
        var action = async () => await Service.Query(new GetCurrentUser.Query());

        // assert
        await action.Should().ThrowAsync<ExecutionContextException>("User ID is not set");
    }
}