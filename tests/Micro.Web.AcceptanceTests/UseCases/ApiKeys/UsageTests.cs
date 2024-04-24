using System.Net.Http.Json;
using FluentAssertions;
using Micro.Web.AcceptanceTests.Pages.ApiKeys;

namespace Micro.Web.AcceptanceTests.UseCases.ApiKeys;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UsageTests : BaseTest
{
    private TestUser _data = null!;

    [SetUp]
    public async Task Setup()
    {
        _data = await Page.GivenLoggedIn();
    }

    [Test]
    public async Task Can_retrieve_current_user()
    {
        // arrange
        var key = await GetApiKey();

        // act
        var client = BuildHttpClient(key);
        
        // assert
        var response = await client.GetFromJsonAsync<UserDto>("/api/users/current");
        response.UserId.Should().Be(_data.UserId);
    }

    private static HttpClient BuildHttpClient(string key)
    {
        var client = new HttpClient
        {
            BaseAddress = Instance.BaseUrl
        };
        client.DefaultRequestHeaders.Add("x-api-key", key);
        return client;
    }

    private async Task<string> GetApiKey()
    {
        var add = await AddPage.Goto(Page);
        await add.AssertPageId();
        await add.EnterName("x");
        await add.ClickSubmit();

        var message = await add.Alert.GetMessage();
        // find value between the single quotes in the message
        var key = message.Split("'")[1];
        return key;
    }

    class UserDto
    {
        public Guid UserId { get; set; }
    }
}