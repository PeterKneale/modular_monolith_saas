using Micro.Translations.Application.Commands;

namespace Micro.Translations.IntegrationTests.Infrastructure;

[Collection(nameof(ServiceFixtureCollection))]
public class BehaviourTests(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Fact]
    public async Task Invalid_command_throws_validation_exception()
    {
        // arrange
        var name = string.Empty;

        // act
        var action = async () => { await Service.Command(new AddTerm.Command(Guid.NewGuid(), name)); };

        // assert
        await action.Should()
            .ThrowAsync<ValidationException>()
            .WithMessage("'Name' must not be empty.");
    }
}