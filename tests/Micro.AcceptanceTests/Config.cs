﻿using Microsoft.Extensions.Configuration;

namespace Micro.AcceptanceTests;

public class Config
{
    private readonly IConfigurationRoot _configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

    public static Config Instance { get; } = new Config();
    
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
    
    // routes
    public const string OrgRoute = "org";
    public const string ProjectRoute = "project";
}