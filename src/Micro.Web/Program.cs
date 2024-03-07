using Micro.Common.Infrastructure.Context;
using Micro.Common.Infrastructure.Database;
using Micro.Translations;
using Micro.Web.Code.Contexts.Authentication;
using Micro.Web.Code.Contexts.Execution;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(c => { c.AddSimpleConsole(x => x.SingleLine = true); });
builder.Services.AddSingleton<ITenantsModule, TenantsModule>();
builder.Services.AddSingleton<ITranslationModule, TranslationModule>();
builder.Services.AddSingleton<IContextAccessor, ContextAccessor>();

builder.Services.AddHttpContextAccessor();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

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

builder.Services.AddSingleton<LoginService>();

// Authentication context
builder.Services.AddScoped<IAuthContext, AuthContext>();

// Page Context
builder.Services.AddScoped<PageContextMiddleware>();
builder.Services.AddScoped<IPageContextAccessor, PageContextAccessor>();
builder.Services.AddScoped<IPageContextOrganisation>(c => c.GetRequiredService<IPageContextAccessor>().Organisation);
builder.Services.AddScoped<IPageContextProject>(c => c.GetRequiredService<IPageContextAccessor>().Project);

var app = builder.Build();
var accessor = app.Services.GetRequiredService<IContextAccessor>();
var logs = app.Services.GetRequiredService<ILoggerProvider>();
TenantsModuleStartup.Start(accessor, configuration);
TranslationModuleStartup.Start(accessor, configuration, logs);

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
app.MapGet("/health/alive", () => "alive");
app.MapGet("/health/ready", () => "ready");
app.MapGet("/test/auth/impersonate/", async ctx =>
{
    // TODO: This is for testing purposes only and will be restricted
    var login = ctx.RequestServices.GetRequiredService<LoginService>();
    var userId = Guid.Parse(ctx.Request.Query["userId"]!);
    await login.Impersonate(userId);
    ctx.Response.Redirect("/");
});
var c = configuration.GetDbConnectionString();
Console.WriteLine(c);
app.Run();