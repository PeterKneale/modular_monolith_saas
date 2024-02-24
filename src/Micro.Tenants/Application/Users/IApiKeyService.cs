using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Application.Users;

public interface IApiKeyService
{
    ApiKeyValue GenerateApiKey();
}