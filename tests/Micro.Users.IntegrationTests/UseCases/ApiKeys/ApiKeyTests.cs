﻿using Micro.Users.Application.ApiKeys.Commands;
using Micro.Users.Application.ApiKeys.Queries;
using CanAuthenticate = Micro.Users.Application.ApiKeys.Queries.CanAuthenticate;

namespace Micro.Users.IntegrationTests.UseCases.ApiKeys;

[Collection(nameof(ServiceFixtureCollection))]
public class ApiKeyTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Can_create_api_key_and_validate_it()
    {
        // arrange
        var userId = Guid.NewGuid();
        var keyId = Guid.NewGuid();

        var register = RegisterCommand(userId);
        await Service.Command(register);

        var createKey = new CreateApiKey.Command(keyId, "x");
        await Service.Command(createKey, userId);

        var getKey = new GetById.Query(keyId);
        var key = await Service.Query(getKey, userId);

        var validateKey = new CanAuthenticate.Query(key);
        var isValid = await Service.Query(validateKey, userId);
        isValid.Valid.Should().BeTrue();
        isValid.UserId.Should().Be(userId);
    }
}