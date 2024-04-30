// Global using directives

global using System.Diagnostics.CodeAnalysis;
global using FluentMigrator;
global using FluentValidation;
global using MediatR;
global using Micro.Common;
global using Micro.Common.Application;
global using Micro.Common.Domain;
global using Micro.Common.Exceptions;
global using Micro.Common.Infrastructure.Database.Converters;
global using Micro.Common.Infrastructure.Integration;
global using Micro.Common.Infrastructure.Integration.Inbox;
global using Micro.Common.Infrastructure.Integration.Outbox;
global using Micro.Common.Infrastructure.Integration.Queue;
global using Micro.Tenants.Application.Organisations;
global using Micro.Tenants.Domain.OrganisationAggregate;
global using Micro.Tenants.Domain.UserAggregate;
global using Micro.Tenants.Infrastructure.Infrastructure.Database;
global using Micro.Tenants.Infrastructure.Infrastructure.Database.Converters;
global using Micro.Tenants.Infrastructure.Infrastructure.Integration.Handlers;
global using Micro.Users.Messages;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Newtonsoft.Json;
global using Quartz;