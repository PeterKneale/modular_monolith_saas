using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;
using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.ApiKeys;

public class ListPage(IPage page) : PageLayout(page)
{
    private static string AddButton => "AddButton";
    private static string DeleteButton => "DeleteButton";
    private static string Row => "Row";
    private static string Name = "Name";

    public async Task<int> GetRowCount() =>
        await Page.GetByTestId(Row).CountAsync();

    public static async Task<ListPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("/user/apikeys");
        return new ListPage(page);
    }

    public async Task<AddPage> ClickAdd()
    {
        await Page
            .GetByTestId(AddButton)
            .ClickAsync();
        var page = new AddPage(Page);
        await page.AssertPageId();
        return page;
    }

    public async Task ClickDelete(int rowNumber)
    {
        await Page.GetByTestId(Row).Nth(rowNumber).GetByTestId(DeleteButton).ClickAsync();
    }

    public async Task<IEnumerable<string>> GetNames()
    {
        var rows = await Page.GetByTestId(Row).AllAsync();
        var list = new List<string>();
        foreach (var row in rows)
        {
            var text = await row.GetByTestId(Name).InnerTextAsync();
            list.Add(text);
        }
        return list;
    }
}