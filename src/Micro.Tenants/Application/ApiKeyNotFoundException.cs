using Micro.Common.Exceptions;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class ApiKeyNotFoundException(UserApiKeyId id) : PlatformException($"ApiKey not found {id}");