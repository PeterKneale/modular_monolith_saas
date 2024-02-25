using FluentAssertions;
using Micro.Tenants.Application.ApiKeys.Commands;
using Micro.Tenants.Application.ApiKeys.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases;

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
        await _service.ExecuteInContext(x => x.SendCommand(register));
        
        var createKey = new CreateUserApiKey.Command(keyId, "x");
        await _service.ExecuteInContext(x => x.SendCommand(createKey), userId:userId);
        
        var getKey = new GetById.Query(keyId);
        var key = await _service.ExecuteInContextQ(x => x.SendQuery(getKey), userId:userId);

        var validateKey = new IsValid.Query(key);
        var isValid = await _service.ExecuteInContextQ(x => x.SendQuery(validateKey), userId:userId);
        isValid.Valid.Should().BeTrue();
        isValid.UserId.Should().Be(userId);
    }
}