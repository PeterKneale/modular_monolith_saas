using Bogus;

namespace Micro.Web.AcceptanceTests.Pages.Auth;

internal record RegisterPageData
{
    private static readonly Faker<RegisterPageData> Fake;

    static RegisterPageData()
    {
        Fake = new Faker<RegisterPageData>()
            .StrictMode(true)
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.Password, (f, u) => f.Internet.Password(memorable: true));
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public static RegisterPageData CreateValid() => Fake.Generate();
}