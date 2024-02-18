namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class OrganisationCreatePage(IPage page) : OrganisationPageLayout(page)
{
    private static string NameField => "Name";
    private static string CreateButton => "Create";

    public static async Task<OrganisationCreatePage> Goto(IPage page)
    {
        await page.GotoAsync(BaseUrl + $"{OrgRoute}/Create");
        return new OrganisationCreatePage(page);
    }

    public async Task Create(string name)
    {
        await page.GetByTestId(NameField).FillAsync(name);
        await page.GetByTestId(CreateButton).ClickAsync();
    }
}