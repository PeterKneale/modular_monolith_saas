using Microsoft.Extensions.Configuration;

namespace Micro.Web.AcceptanceTests;

public class Config
{
    // routes
    public const string OrgRoute = "org";
    public const string ProjectRoute = "project";

    private readonly IConfigurationRoot _configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

    public static Config Instance { get; } = new();

    public Uri BaseUrl
    {
        get
        {
            var scheme = _configuration["WEB_SCHEME"] ?? "http";
            var host = _configuration["WEB_HOST"] ?? "localhost";
            var port = _configuration["WEB_PORT"] ?? "8080";
            return new Uri($"{scheme}://{host}:{port}");
        }
    }

    public Uri AliveEndpoint => new(BaseUrl, "/health/alive");

    public Uri ReadyEndpoint => new(BaseUrl, "/health/ready");
}