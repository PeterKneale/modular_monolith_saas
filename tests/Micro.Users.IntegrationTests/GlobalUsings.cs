global using Xunit;
global using FluentAssertions;
global using Micro.Common.Application;
global using Micro.Common.Domain;
global using Micro.Common.Exceptions;
global using Micro.Common.Infrastructure.Context;
global using Micro.Users.Application.Users.Commands;
global using Micro.Users.Application.Users.Queries;
global using Micro.Users.IntegrationTests.Fixtures;
global using Microsoft.Extensions.Configuration;
global using Xunit;
global using Xunit.Abstractions;
using System.Diagnostics.CodeAnalysis;
[assembly:ExcludeFromCodeCoverage]
[assembly: AssemblyTrait("Type", "Integration")]