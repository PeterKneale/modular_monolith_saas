namespace Micro.Users.Application.Users;

public interface IApiKeyService
{
    ApiKeyValue GenerateApiKey();
}