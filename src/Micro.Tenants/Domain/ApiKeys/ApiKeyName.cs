﻿namespace Micro.Tenants.Domain.ApiKeys;

public record ApiKeyName(string Value)
{
    public override string ToString() => $"{Value}";
}