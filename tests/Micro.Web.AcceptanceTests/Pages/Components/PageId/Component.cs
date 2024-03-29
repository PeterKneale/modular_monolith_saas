namespace Micro.Web.AcceptanceTests.Pages.Components.PageId;

public class Component(IPage page)
{
    private readonly ILocator _pageId = page.GetByTestId("PageId");

    public async Task<string> GetPageId()
    {
        var value = await _pageId.GetAttributeAsync("data-value");
        if (value is null)
        {
            throw new Exception("PageId not found on page");
        }
        return value;
    }
}