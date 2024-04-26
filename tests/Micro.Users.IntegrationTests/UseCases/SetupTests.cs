namespace Micro.Users.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class SetupTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Setup_user_for_manual_testing()
    {
        // arrange
        var email = GetUniqueEmail();
        var password = "password";

        // act
        var userId = await GivenVerifiedUser(email, password);
        var results = await Service.Query(new CanAuthenticate.Query(email, password));

        // assert
        results.Success.Should().BeTrue();
        results.UserId.Should().Be(userId);
        var impersonateUri = $"http://localhost:8080/test/auth/impersonate?userId={userId}";
        Output.WriteLine($"Impersonate {email} with link: {impersonateUri}");
    }
}