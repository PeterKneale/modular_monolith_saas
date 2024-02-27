﻿using FluentAssertions;
using Micro.Tenants.Application.Users.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ServiceFixtureCollection))]
public class RegistrationTests
{
    private readonly ServiceFixture _service;

    public RegistrationTests(ServiceFixture service, ITestOutputHelper outputHelper)
    {
        service.OutputHelper = outputHelper;
        _service = service;
    }

    [Fact]
    public async Task Registering_allows_login()
    {
        // arrange
        var userId = Guid.NewGuid();
        var register = Build.RegisterCommand(userId);
        var login = new CanAuthenticate.Query(register.Email, register.Password);

        // act
        await _service.Command(register);
        var results = await _service.Query(login);

        // assert
        results.Success.Should().BeTrue();
        results.UserId.Should().Be(userId);
    }
}