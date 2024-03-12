﻿namespace Micro.Tenants.Domain.Projects;

public record ProjectName
{
    private ProjectName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ProjectName CreateInstance(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Project Name must not be empty", nameof(value));

        value = NameSanitizer.SanitizedValue(value);

        return new ProjectName(value);
    }

    public override string ToString() => $"{Value}";
}