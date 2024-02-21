using Micro.Common.Infrastructure.Context;
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

// Authentication context
builder.Services.AddScoped<IAuthContext, AuthContext>();

// Page Context
builder.Services.AddScoped<PageContextMiddleware>();
builder.Services.AddScoped<IPageContextAccessor, PageContextAccessor>();
builder.Services.AddScoped<IPageContextOrganisation>(c => c.GetRequiredService<IPageContextAccessor>().Organisation);
builder.Services.AddScoped<IPageContextProject>(c => c.GetRequiredService<IPageContextAccessor>().Project);

var app = builder.Build();
var accessor = app.Services.GetRequiredService<IContextAccessor>();
TenantsModuleStartup.Start(accessor, configuration);
TranslationModuleStartup.Start(accessor, configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.Run();