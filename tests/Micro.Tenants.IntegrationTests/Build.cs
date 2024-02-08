using Bogus;
using Micro.Tenants.Application.Auth;

namespace Micro.Tenants.IntegrationTests;

public static class Build
{
    public static Register.Command BuildRegisterCommand(Guid organisationId, Guid userId, string? email = null, string? password = null)
    {
        return new Faker<Register.Command>()
            .CustomInstantiator(f =>
                new Register.Command(
                    organisationId,
                    f.Company.CompanyName(),
                    userId,
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    email ?? f.Internet.Email(),
                    password ?? f.Internet.Password(15)
                )
            ).Generate();
    }
}