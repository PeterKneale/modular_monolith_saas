global using FluentAssertions;
global using JetBrains.Annotations;
global using Micro.Common.Application;
global using Micro.Common.Exceptions;
global using Micro.Common.Infrastructure.Context;
global using Micro.Translations.IntegrationTests.Fixtures;
global using Microsoft.Extensions.Configuration;
global using Xunit;
global using Xunit.Abstractions;
global using static Micro.Translations.IntegrationTests.Fixtures.TestData;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]
[assembly: AssemblyTrait("Type", "Integration")]