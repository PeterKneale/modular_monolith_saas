﻿namespace Micro.Users.IntegrationTests.Fixtures;

public abstract class BaseTest
{
    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        Output = output;
        service.OutputHelper = output;
        Service = service;
    }

    protected ServiceFixture Service { get; }

    protected ITestOutputHelper Output { get; set; }

    protected async Task<Guid> GivenVerifiedUser(string? email = null, string? password = null)
    {
        var userId = await GivenRegisteredUser(email, password);
        var token = await Service.Query(new GetUserVerificationToken.Query(userId));
        await Service.Command(new VerifyUser.Command(userId, token));
        return userId;
    }

    protected async Task<Guid> GivenRegisteredUser(string? email = null, string? password = null)
    {
        var userId = Guid.NewGuid();
        var register = RegisterCommand(userId, email, password);
        await Service.Command(register);
        return userId;
    }
}