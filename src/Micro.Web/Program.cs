using Micro.Common.Infrastructure;
using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Database;
using Micro.Common.Infrastructure.Integration.Bus;
using Micro.Common.Web.Contexts.AuthContext;
using Micro.Common.Web.Contexts.PageContext;
using Micro.Tenants.Web;
using Micro.Users.Application.Users.Queries;
using Micro.Translations.Infrastructure;
using Micro.Users.Web;
using Micro.Users.Web.Contexts.Authentication;
using Micro.Web.Api.Users;
using Micro.Web.Code.Contexts.ExecutionContext;
using Micro.Web.Code.Contexts.PageContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(c => { c.AddSimpleConsole(x => { x.SingleLine = true; }); });

// Data protection keys, necessary for multiple instances of the web server
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "Files");
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName(nameof(Micro.Web));

// Add services to the container.
builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AllowAnonymousToPage("/Auth/Forbidden");
        options.Conventions.AllowAnonymousToPage("/Auth/ForgotPassword");
        options.Conventions.AllowAnonymousToPage("/Auth/ResetPassword");
        options.Conventions.AllowAnonymousToPage("/Auth/Login");
        options.Conventions.AllowAnonymousToPage("/Auth/Logout");
        options.Conventions.AllowAnonymousToPage("/Auth/Register");
        //TODO: find out why The request verification token is not added for pages served from assemblies
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    });
builder.Services
    .AddControllersWithViews()
    .AddApplicationPart(UsersWebAssemblyInfo.Assembly)
    .AddApplicationPart(TenantsWebAssemblyInfo.Assembly)
    .AddRazorRuntimeCompilation();
// see: https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-8.0
builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{
    options.FileProviders.Add(new EmbeddedFileProvider(UsersWebAssemblyInfo.Assembly));
    options.FileProviders.Add(new EmbeddedFileProvider(TenantsWebAssemblyInfo.Assembly));
});

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

builder.Services.AddSingleton<AuthenticationService>();

// Authentication
builder.Services.AddScoped<ApiKeyAuthenticationMiddleware>();

// Authentication context
builder.Services.AddScoped<IAuthContext, AuthContext>();

// Page Context
builder.Services.AddScoped<PageContextMiddleware>();
builder.Services.AddScoped<IPageContextAccessor, PageContextAccessor>();
builder.Services.AddScoped<IPageContextOrganisation>(c => c.GetRequiredService<IPageContextAccessor>().Organisation);
builder.Services.AddScoped<IPageContextProject>(c => c.GetRequiredService<IPageContextAccessor>().Project);

// Execution context
builder.Services.AddSingleton<IExecutionContextAccessor, HttpExecutionContextAccessor>();

// modules
builder.Services.AddSingleton<IUsersModule, UsersModule>();
builder.Services.AddSingleton<ITenantsModule, TenantsModule>();
builder.Services.AddSingleton<ITranslationModule, TranslationModule>();

builder.Services.AddInMemoryEventBus();

var app = builder.Build();
var accessor = app.Services.GetRequiredService<IExecutionContextAccessor>();

var bus = app.Services.GetRequiredService<IEventsBus>();
var logs = app.Services.GetRequiredService<ILoggerFactory>();

var schedules = configuration.IsSchedulerEnabled();
var migrations = configuration.IsMigrationEnabled();
await UsersModuleStartup.Start(accessor, configuration, bus, logs, enableMigrations: migrations, enableScheduler: schedules);
await TenantsModuleStartup.Start(accessor, configuration, bus, logs, enableMigrations: migrations, enableScheduler: schedules);
await TranslationModuleStartup.Start(accessor, configuration, bus, logs, enableMigrations: migrations, enableScheduler: schedules);

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
app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
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

var usersApi = app.MapGroup("/api/users");
usersApi.MapGet("/current", User.GetCurrentUser);

app.MapGet("/Test/Auth/Impersonate/", async ctx =>
{
    var login = ctx.RequestServices.GetRequiredService<AuthenticationService>();
    var userId = Guid.Parse(ctx.Request.Query["userId"]!);
    await login.Impersonate(userId);
    ctx.Response.Redirect("/");
});
app.MapGet("/Test/GetUserId", async ctx =>
{
    var module = ctx.RequestServices.GetRequiredService<IUsersModule>();
    var email = ctx.Request.Query["email"]!;
    var userId = await module.SendQuery(new GetUserIdByEmail.Query(email!));
    await ctx.Response.WriteAsync(userId.ToString());
});
app.MapGet("/Test/GetUserVerificationToken", async ctx =>
{
    var module = ctx.RequestServices.GetRequiredService<IUsersModule>();
    var userId = Guid.Parse(ctx.Request.Query["userId"]!);
    var token = await module.SendQuery(new GetUserVerificationToken.Query(userId));
    await ctx.Response.WriteAsync(token);
});
app.MapGet("/Test/GetPasswordResetToken", async ctx =>
{
    var module = ctx.RequestServices.GetRequiredService<IUsersModule>();
    var userId = Guid.Parse(ctx.Request.Query["userId"]!);
    var token = await module.SendQuery(new GetResetPasswordToken.Query(userId));
    await ctx.Response.WriteAsync(token);
});

app.Run();