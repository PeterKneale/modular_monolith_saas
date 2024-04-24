namespace Micro.Web.AcceptanceTests;

public abstract class BaseTest : IDisposable
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;

    protected BaseTest()
    {
        Setup().GetAwaiter().GetResult();
    }

    protected IPage Page { get; private set; } = null!;

    private async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            //SlowMo = 100
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