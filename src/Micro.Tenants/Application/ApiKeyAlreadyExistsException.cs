using Micro.Common.Exceptions;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application;

[ExcludeFromCodeCoverage]
public class ApiKeyAlreadyExistsException(UserApiKeyId id) : PlatformException($"ApiKey already exists {id}");