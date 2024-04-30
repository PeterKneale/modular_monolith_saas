global using FluentAssertions;
global using Micro.Common.Application;
global using Micro.Common.Exceptions;
global using Micro.Common.Infrastructure.Context;
global using Micro.Tenants.IntegrationTests.Fixtures;
global using Micro.Tenants.Messages;
global using Micro.Users.Messages;
global using Microsoft.Extensions.Configuration;
global using Xunit;
global using Xunit.Abstractions;
using System.Diagnostics.CodeAnalysis;

[assembly:ExcludeFromCodeCoverage]
[assembly: AssemblyTrait("Type", "Integration")]