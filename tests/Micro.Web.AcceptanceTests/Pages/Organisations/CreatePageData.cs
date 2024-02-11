using Bogus;

namespace Micro.Web.AcceptanceTests.Pages.Organisations;

public class CreatePageData
{
    private static readonly Faker<CreatePageData> Fake;

    static CreatePageData()
    {
        Fake = new Faker<CreatePageData>()
            .StrictMode(true)
            .RuleFor(field => field.Name, use => use.Company.CompanyName()
                .Replace("_", string.Empty)
                .Replace(" ",  string.Empty));
    }

    public string Name { get; set; }

    public static CreatePageData CreateValid() => Fake.Generate();
}