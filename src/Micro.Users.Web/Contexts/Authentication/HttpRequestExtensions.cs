namespace Micro.Users.Web.Contexts.Authentication;

public static class HttpRequestExtensions
{
    public static Uri GetBaseUrl(this HttpRequest req)
    {
        var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
        if (uriBuilder.Uri.IsDefaultPort) uriBuilder.Port = -1;

        return uriBuilder.Uri;
    }
}