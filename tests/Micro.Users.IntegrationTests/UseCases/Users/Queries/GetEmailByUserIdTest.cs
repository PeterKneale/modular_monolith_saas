namespace Micro.Users.IntegrationTests.UseCases.Users.Queries;

[Collection(nameof(ServiceFixtureCollection))]
public class GetEmailByUserIdTest(ServiceFixture service, ITestOutputHelper outputHelper) : BaseTest(service, outputHelper)
{
    [Theory]
    [InlineData("kytpthmri@example.org")]
    [InlineData("ABSDBHASD@example.org")]
    public async Task Email_can_be_retrieved(string email)
    {
        // arrange
        var password = "password";
        var userId = await GivenVerifiedUser(email, password);

        // act
        var result = await Service.Query(new GetEmailByUserId.Query(userId));

        // assert
        result.Canonical.Should().Be(email.ToLowerInvariant());
        result.Display.Should().Be(email);
    }

    [Fact]
    public async Task Throws_not_found_exception_when_not_found()
    {
        // arrange
        var userId = Guid.NewGuid();

        // act
        var act = async () => { await Service.Query(new GetEmailByUserId.Query(userId)); };

        // assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}