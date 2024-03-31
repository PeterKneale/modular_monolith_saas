using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Database;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Users.Application.Users.Commands;
using Micro.Users.Application.Users.Queries;
using Micro.Translations;
using Micro.Users;
using Micro.Web.Code.Contexts.Authentication;
using Micro.Web.Code.Contexts.Execution;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging.Console;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(c => { c.AddSimpleConsole(x =>
{
    x.SingleLine = false;
    x.ColorBehavior = LoggerColorBehavior.Enabled;
}); });

// Add services to the container.
builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AllowAnonymousToPage("/Auth/Forbidden");
        options.Conventions.AllowAnonymousToPage("/Auth/Forgot");
        options.Conventions.AllowAnonymousToPage("/Auth/Login");
        options.Conventions.AllowAnonymousToPage("/Auth/Logout");
        options.Conventions.AllowAnonymousToPage("/Auth/Register");
    })
    .AddRazorRuntimeCompilation();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Forbidden";
    });
builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        connectionString: configuration.GetDbConnectionString("public"),
        healthQuery: "SELECT 1;",
        name: "sql",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db" });

builder.Services.AddSingleton<LoginService>();

// Authentication context
builder.Services.AddScoped<IAuthContext, AuthContext>();

// Page Context
builder.Services.AddScoped<PageContextMiddleware>();
builder.Services.AddScoped<IPageContextAccessor, PageContextAccessor>();
builder.Services.AddScoped<IPageContextOrganisation>(c => c.GetRequiredService<IPageContextAccessor>().Organisation);
builder.Services.AddScoped<IPageContextProject>(c => c.GetRequiredService<IPageContextAccessor>().Project);

// Execution context
builder.Services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

// modules
builder.Services.AddSingleton<IUsersModule, UsersModule>();
builder.Services.AddSingleton<ITenantsModule, TenantsModule>();
builder.Services.AddSingleton<ITranslationModule, TranslationModule>();

builder.Services.AddInMemoryEventBus();

var app = builder.Build();
var accessor = app.Services.GetRequiredService<IExecutionContextAccessor>();

var bus = app.Services.GetRequiredService<IEventsBus>();
var logs = app.Services.GetRequiredService<ILoggerFactory>();

await UsersModuleStartup.Start(accessor, configuration, bus, logs);
await TenantsModuleStartup.Start(accessor, configuration, bus, logs);
await TranslationModuleStartup.Start(accessor, configuration, bus, logs);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseMiddleware<PageContextMiddleware>();
app.MapHealthChecks("/health/alive", new HealthCheckOptions
{
    Predicate = _ => true
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = checks => checks.Tags.Contains("db"),
});
app.MapGet("/Test/Auth/Impersonate/", async ctx =>
{
    var login = ctx.RequestServices.GetRequiredService<LoginService>();
    var userId = Guid.Parse(ctx.Request.Query["userId"]!);
    await login.Impersonate(userId);
    ctx.Response.Redirect("/");
});
app.MapGet("/Test/GetUserId", async ctx =>
{
    var module = ctx.RequestServices.GetRequiredService<IUsersModule>();
    var email = ctx.Request.Query["email"]!;
    var userId = await module.SendQuery(new GetUserId.Query(email!));
    await ctx.Response.WriteAsync(userId.ToString());
});
app.MapGet("/Test/GetUserVerification", async ctx =>
{
    var module = ctx.RequestServices.GetRequiredService<IUsersModule>();
    var userId = Guid.Parse(ctx.Request.Query["userId"]!);
    var token = await module.SendQuery(new GetUserVerificationToken.Query(userId));
    await ctx.Response.WriteAsync(token);
});

app.Run();