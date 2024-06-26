using System.Security.Claims;
using Micro.Users.Application.Users.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Micro.Users.Web.Contexts.Authentication;

public class AuthenticationService(IUsersModule module, IHttpContextAccessor accessor, ILogger<AuthenticationService> logs)
{
    public async Task<bool> AuthenticateWithCredentials(string email, string password)
    {
        var result = await module.SendQuery(new CanAuthenticate.Query(email, password));
        if (result.Success == false)
        {
            logs.LogWarning("Authentication was not successful: {Email}", email);
            return false;
        }

        if (!result.UserId.HasValue)
        {
            logs.LogWarning("Authentication was not successful, no user id returned: {Email}", email);
            return false;
        }

        var userId = result.UserId.Value;

        await Authenticate(userId, email);

        logs.LogInformation("Authentication was successful: {Email}", email);

        return true;
    }

    public async Task Impersonate(Guid userId)
    {
        var email = await module.SendQuery(new GetEmailByUserId.Query(userId));
        await Authenticate(userId, email.Canonical);
    }

    private async Task Authenticate(Guid userId, string email)
    {
        var claims = new Claim[]
        {
            new(AuthConstants.UserIdKey, userId.ToString()),
            new(AuthConstants.UserEmailKey, email)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await accessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        LogImpersonationLink(userId, email);
    }

    // TODO
    // Ensure this is only in local debug builds
    private void LogImpersonationLink(Guid userId, string email)
    {
        var baseUri = accessor.HttpContext!.Request.GetBaseUrl();
        var impersonateUri = new Uri(baseUri, new Uri($"/test/auth/impersonate?userId={userId}", UriKind.Relative));
        logs.LogInformation("Impersonate {Email} with link: {LoginUri}", email, impersonateUri);
    }
}