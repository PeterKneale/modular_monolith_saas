using Micro.Common.Exceptions;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class ApiKeyNameInUseException(ApiKeyName name) : PlatformException($"ApiKey name in use {name}");