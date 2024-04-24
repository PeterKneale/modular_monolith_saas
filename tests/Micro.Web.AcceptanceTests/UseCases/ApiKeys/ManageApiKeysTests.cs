﻿using FluentAssertions;
using Micro.Web.AcceptanceTests.Extensions;
using Micro.Web.AcceptanceTests.Pages.ApiKeys;
using Micro.Web.AcceptanceTests.Pages.Components.PageId;

namespace Micro.Web.AcceptanceTests.UseCases.ApiKeys;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ManageApiKeysTests : BaseTest
{
    [SetUp]
    public async Task Setup()
    {
        await Page.GivenLoggedIn();
    }
    
    [Test]
    public async Task Can_show_initial_state()
    {
        await ListShouldBeEmpty();
    }

    [Test]
    public async Task Shown_in_creation_order()
    {
        // arrange
        var names = new List<string> { "b", "a", "c" };

        // act
        await AddToList(names[0]);
        await AddToList(names[1]);
        await AddToList(names[2]);

        // assert
        await ListShouldContain(names);
    }

    private async Task ListShouldContain(IEnumerable<string> names)
    {
        var list = await ListPage.Goto(Page);
        var actual = await list.GetNames();
        actual.Should().BeEquivalentTo(names.OrderBy(x => x));
    }

    [Test]
    public async Task Can_add_and_remove()
    {
        await Page.GivenLoggedIn();

        // add records
        await AddToList("X");
        await ListCountShouldBe(1);
        await AddToList("Y");
        await ListCountShouldBe(2);
        await AddToList("Z");
        await ListCountShouldBe(3);

        // remove records
        var list = await ListPage.Goto(Page);
        await list.ClickDelete(0);
        await ListCountShouldBe(2);
        await list.ClickDelete(0);
        await ListCountShouldBe(1);
        await list.ClickDelete(0);
        await ListShouldBeEmpty();
    }

    private async Task AddToList(string? name = null)
    {
        var list = await ListPage.Goto(Page);
        var add = await list.ClickAdd();
        await add.AssertPageId();
        await add.EnterName(name ?? Guid.NewGuid().ToString()[6..]);
        await add.ClickSubmit();
    }

    private async Task ListShouldBeEmpty()
    {
        var list = await ListPage.Goto(Page);
        var count = await list.GetRowCount();
        count.Should().Be(0);
    }

    private async Task ListCountShouldBe(int total)
    {
        var list = await ListPage.Goto(Page);
        var count = await list.GetRowCount();
        count.Should().Be(total);
    }
}