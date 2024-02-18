using Bogus;

namespace Micro.Web.AcceptanceTests.UseCases.Auth;

public record RegisterPageData
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

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public static RegisterPageData CreateValid() => Fake.Generate();
}