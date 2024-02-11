namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class CreatePage(IPage page)
{
    private static string NameField => "Name";
    private static string CreateButton => "Create";

    public static CreatePage Goto(IPage page)
    {
        page.GotoAsync(Constants.BaseUrl + "Organisations/Create");
        return new CreatePage(page);
    }

    public async Task Create(string name)
    {
        await page.GetByTestId(NameField).FillAsync(name);
        await page.GetByTestId(CreateButton).ClickAsync();
    }
}