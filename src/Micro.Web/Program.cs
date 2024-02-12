using Micro.Common.Application;
using Micro.Common.Infrastructure.Context;
using Micro.Tenants.Application.Organisations;
using Micro.Translations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Constants = Micro.Web.Code.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITenantsModule, TenantsModule>();
builder.Services.AddSingleton<ITranslationModule, TranslationModule>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IContextAccessor, ContextAccessor>();

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

app.Use(async (ctx, next) =>
{
    // This is a middleware that will run for every request
    // It will check if the request has an organisation route value
    // If so, it will get the organisation from the database and add it to the HttpContext.Items
    var module = ctx.RequestServices.GetRequiredService<ITenantsModule>();
    if (ctx.Request.RouteValues.TryGetValue(Constants.OrgRouteKey, out var org))
    {
        var name = org!.ToString()!;
        var result = await module.SendQuery(new GetOrganisationByName.Query(name));
        ctx.Request.HttpContext.SetOrganisationId(result.Id);
    }

    await next.Invoke();
});
app.MapGet("/health/alive", () => "alive");
app.MapGet("/health/ready", () => "ready");

app.Run();