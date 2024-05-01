using Bogus;

namespace Micro.Users.IntegrationTests;

public static class TestData
{
    public static RegisterUser.Command RegisterCommand(Guid userId, string? email = null, string? password = null)
    {
        return new Faker<RegisterUser.Command>()
            .CustomInstantiator(f =>
                new RegisterUser.Command(
                    userId,
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    email ?? f.Internet.Email(),
                    password ?? f.Internet.Password(15)
                )
            ).Generate();
    }
}