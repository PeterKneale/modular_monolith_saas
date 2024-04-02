using Micro.Web.AcceptanceTests.Extensions;

namespace Micro.Web.AcceptanceTests;

public abstract class BaseTest : IDisposable
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;

    protected BaseTest(bool debug = false)
    {
        Setup(debug).GetAwaiter().GetResult();
    }

    protected IPage Page { get; private set; } = null!;

    private async Task Setup(bool debug)
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = !debug,
            SlowMo = debug ? 0 : 0
        });
        Page = await _browser.NewPageAsync();

        await EventuallyHelper.ShouldPass(async () =>
        {
            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(1);
            (await http.GetAsync(Instance.AliveEndpoint)).EnsureSuccessStatusCode();
            (await http.GetAsync(Instance.ReadyEndpoint)).EnsureSuccessStatusCode();
        }, "Web server is not alive and ready");
    }

    public void Dispose()
    {
        _browser?.DisposeAsync().GetAwaiter().GetResult();
        _playwright?.Dispose();
    }
}