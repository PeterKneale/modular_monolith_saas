namespace Micro.Web.AcceptanceTests.Pages.Layouts;

public class PageLayout(IPage page)
{
    protected readonly IPage Page = page;

    public PageId PageId => new(Page);

    public Menu Menu => new(Page);

    public AlertComponent Alert => new(Page);
}