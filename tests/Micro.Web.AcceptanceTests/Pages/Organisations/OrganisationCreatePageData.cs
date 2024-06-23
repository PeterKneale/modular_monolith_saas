using Bogus;

namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class OrganisationCreatePageData
{
    private static readonly Faker<OrganisationCreatePageData> Fake;

    static OrganisationCreatePageData()
    {
        Fake = new Faker<OrganisationCreatePageData>()
            .StrictMode(true)
            .RuleFor(field => field.Name, use => (use.Company.CompanyName() + Guid.NewGuid().ToString()[..6])
                .Replace("_", string.Empty)
                .Replace(" ", string.Empty)
                .Replace(",", string.Empty)
                .Replace("'", string.Empty)
                .Replace("-", string.Empty));
    }

    public string Name { get; init; } = null!;

    public static OrganisationCreatePageData CreateValid() => Fake.Generate();
}