using Micro.Users.Application.ApiKeys.Commands;

namespace Micro.Users.IntegrationTests.Infrastructure.Context;

[Collection(nameof(ServiceFixtureCollection))]
public class ContextTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Performing_an_operation_without_user_context_throws()
    {
        // arrange
        var userId = Guid.NewGuid();
        var keyId = Guid.NewGuid();

        var register = RegisterCommand(userId);
        await Service.Command(register);

        // act 
        var action = async () =>
        {
            // note the lack of userId context
            await Service.Command(new CreateApiKey.Command(keyId, "x"));
        };

        await action
            .Should()
            .ThrowAsync<ExecutionContextException>()
            .WithMessage("User ID is not set in the execution context");
    }
}