namespace Micro.Tenants.IntegrationTests.Infrastructure;

[Collection(nameof(ServiceFixtureCollection))]
public class BehaviourTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Invalid_email_address_fails_validation()
    {
        // arrange
        var email = "test";
        var password = "password";
        
        // act
        var action = async () => { await RegisterUser(email, password); };
        
        // assert
        await action.Should().ThrowAsync<ValidationException>();
    }
}