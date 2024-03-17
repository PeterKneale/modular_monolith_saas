﻿using Bogus;

namespace Micro.AcceptanceTests.Pages.Projects;

public record ProjectCreatePageData
{
    public string Name { get; init; } = null!;

    private static readonly Faker<ProjectCreatePageData> Fake;

    static ProjectCreatePageData()
    {
        Fake = new Faker<ProjectCreatePageData>()
            .StrictMode(true)
            .RuleFor(field => field.Name, use => (use.Hacker.Adjective() + Guid.NewGuid().ToString()[..6])
                .Replace("_", string.Empty)
                .Replace(" ", string.Empty));
    }

    public static ProjectCreatePageData CreateValid() => Fake.Generate();
}