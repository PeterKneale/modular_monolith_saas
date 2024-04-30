namespace Micro.Web.AcceptanceTests.UseCases;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SmokeTests : BaseTest
{
    [Test]
    public async Task Can_login()
    {
        await Page.GivenLoggedIn();
    }
}