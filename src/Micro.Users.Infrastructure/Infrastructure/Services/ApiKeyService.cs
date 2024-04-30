using System.Security.Cryptography;
using Micro.Users.Domain.ApiKeys;
using Micro.Users.Domain.ApiKeys.Services;

namespace Micro.Users.Infrastructure.Infrastructure.Services;

internal class ApiKeyService : IApiKeyService
{
    private const string Prefix = "MS-";
    private const int NumberOfSecureBytesToGenerate = 32;
    private const int LengthOfKey = 32;

    public ApiKeyValue GenerateApiKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(NumberOfSecureBytesToGenerate);

        var base64String = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_");

        var keyLength = LengthOfKey - Prefix.Length;

        var key = Prefix + base64String[..keyLength];
        return new ApiKeyValue(key);
    }
}