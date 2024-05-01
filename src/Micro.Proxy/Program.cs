var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(c => { c.AddSimpleConsole(x => { x.SingleLine = true; }); });
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.MapGet("/health/alive", () => "alive");
app.MapGet("/health/ready", () => "ready");
app.MapReverseProxy();
app.Run();