using Micro.Tenants.Application.ApiKeys.Commands;
using Micro.Tenants.Application.ApiKeys.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.ApiKeys;

[Collection(nameof(ServiceFixtureCollection))]
public class ApiKeyTests
{
    private readonly ServiceFixture _service;

    public ApiKeyTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Can_create_api_key_and_validate_it()
    {
        // arrange
        var userId = Guid.NewGuid();
        var keyId = Guid.NewGuid();

        var register = Build.RegisterCommand(userId);
        await _service.Command(register);

        var createKey = new CreateUserApiKey.Command(keyId, "x");
        await _service.Command(createKey, userId);

        var getKey = new GetUserApiKeyById.Query(keyId);
        var key = await _service.Query(getKey, userId);

        var validateKey = new CanAuthenticateWithApiKey.Query(key);
        var isValid = await _service.Query(validateKey, userId);
        isValid.Valid.Should().BeTrue();
        isValid.UserId.Should().Be(userId);
    }
}