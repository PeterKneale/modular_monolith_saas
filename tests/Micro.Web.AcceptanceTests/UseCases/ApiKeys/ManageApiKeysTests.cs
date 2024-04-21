using FluentAssertions;
using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.ApiKeys;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;

namespace Micro.Web.AcceptanceTests.UseCases.ApiKeys;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ManageApiKeysTests : BaseTest
{
    [Test]
    public async Task Can_show_initial_state()
    {
        await Page.GivenLoggedIn();
        var list = await ListPage.Goto(Page);
        await ListShouldBeEmpty(list);
    }

    [Test]
    public async Task Shown_in_creation_order()
    {
        await Page.GivenLoggedIn();
        var list = await ListPage.Goto(Page);
        var names = new List<string> { "b", "a", "c" };

        // add records
        list = await AddOne(list,names[0]);
        list = await AddOne(list,names[1]);
        list = await AddOne(list,names[2]);

        // assert order
        var actual = await list.GetNames();
        actual.Should().BeEquivalentTo(names.OrderBy(x => x));
    }

    [Test]
    public async Task Can_add_and_remove()
    {
        await Page.GivenLoggedIn();
        var list = await ListPage.Goto(Page);

        // add records
        list = await AddOne(list);
        await ListShouldHave(list, 1);
        list = await AddOne(list);
        await ListShouldHave(list, 2);
        list = await AddOne(list);
        await ListShouldHave(list, 3);
        
        // remove records
        await list.ClickDelete(0);
        await ListShouldHave(list, 2);
        await list.ClickDelete(0);
        await ListShouldHave(list, 1);
        await list.ClickDelete(0);
        await ListShouldBeEmpty(list);
    }

    private static async Task<ListPage> AddOne(ListPage list, string? name = null)
    {
        var addPage = await list.ClickAdd();
        await addPage.AssertPageId();
        await addPage.EnterName(name ?? Guid.NewGuid().ToString()[6..]);
        return await addPage.ClickSubmit();
    }

    private static async Task ListShouldBeEmpty(ListPage list)
    {
        var count = await list.GetRowCount();
        count.Should().Be(0);
    }

    private static async Task ListShouldHave(ListPage list, int total)
    {
        var count = await list.GetRowCount();
        count.Should().Be(total);
    }
}