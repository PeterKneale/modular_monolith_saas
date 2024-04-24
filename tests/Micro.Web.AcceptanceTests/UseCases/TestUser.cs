using Bogus;

namespace Micro.Web.AcceptanceTests.UseCases;

public record TestUser
{
    private static readonly Faker<TestUser> Fake;

    static TestUser()
    {
        Fake = new Faker<TestUser>()
            .StrictMode(true)
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.Password, (f, u) => f.Internet.Password(memorable: true, length: 20))
            .Ignore(u => u.UserId);
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; private set; } = null!;
    public Guid UserId { get; init; }

    public void RegeneratePassword() => Password = Fake.Generate().Password;

    public static TestUser CreateValid() => Fake.Generate();
}