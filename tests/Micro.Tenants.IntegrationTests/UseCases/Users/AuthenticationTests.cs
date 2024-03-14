﻿using Micro.Tenants.Application.Users.Queries;

namespace Micro.Tenants.IntegrationTests.UseCases.Users;

[Collection(nameof(ServiceFixtureCollection))]
public class AuthenticationTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Registering_and_verification_allows_login()
    {
        // arrange
        var email = $"test{Guid.NewGuid().ToString()}@example.org";
        var password = "password";
        
        // act
        var userId = await RegisterAndVerifyUser(email, password);
        var results = await Service.Query(new CanAuthenticate.Query(email, password));

        // assert
        results.Success.Should().BeTrue();
        results.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Unverified_users_cannot_login()
    {
        // arrange
        var email = $"test{Guid.NewGuid().ToString()}@example.org";
        var password = "password";
        
        // act
        await RegisterUser(email, password);
        var results = await Service.Query(new CanAuthenticate.Query(email, password));

        // assert
        results.Success.Should().BeFalse();
    }
}