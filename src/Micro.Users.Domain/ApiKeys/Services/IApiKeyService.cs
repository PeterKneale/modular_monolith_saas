namespace Micro.Users.Domain.ApiKeys.Services;

public interface IApiKeyService
{
    ApiKeyValue GenerateApiKey();
}