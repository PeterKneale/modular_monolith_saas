using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class OrganisationCreatePage(IPage page) : OrganisationPageLayout(page)
{
    private static string NameField => "Name";
    private static string CreateButton => "Create";

    public static async Task<OrganisationCreatePage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync($"{OrgRoute}/Create");
        return new OrganisationCreatePage(page);
    }

    public async Task Create(string name)
    {
        await Page.GetByTestId(NameField).FillAsync(name);
        await Page.GetByTestId(CreateButton).ClickAsync();
    }
}