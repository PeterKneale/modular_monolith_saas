using Micro.Common.Domain;
using Micro.Tenants.Application.Users.Commands;

namespace Micro.Tenants.IntegrationTests.UseCases.Users;

[Collection(nameof(ServiceFixtureCollection))]
public class VerificationTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Verification_token_must_be_correct()
    {
        // arrange
        var email = $"test{Guid.NewGuid().ToString()}@example.org";
        var password = "password";
        
        // act
        var userId = await RegisterUser(email, password);
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        var act = async () => { await Service.Command(new VerifyUser.Command(userId, token + "x")); };

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("*token*");
    }
}