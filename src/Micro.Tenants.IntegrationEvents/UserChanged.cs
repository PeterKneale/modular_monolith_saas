﻿using Micro.Common.Infrastructure.Integration;

namespace Micro.Tenants.IntegrationEvents;

public class UserChanged : IntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}