using Bogus;

namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class OrganisationCreatePageData
{
    public string Name { get; init; }
    
    private static readonly Faker<OrganisationCreatePageData> Fake;

    static OrganisationCreatePageData()
    {
        Fake = new Faker<OrganisationCreatePageData>()
            .StrictMode(true)
            .RuleFor(field => field.Name, use => use.Company.CompanyName()
                .Replace("_", string.Empty)
                .Replace(" ",  string.Empty));
    }

    public static OrganisationCreatePageData CreateValid() => Fake.Generate();
}