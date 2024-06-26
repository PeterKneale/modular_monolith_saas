﻿using Micro.Web.AcceptanceTests.Pages.Layouts;

namespace Micro.Web.AcceptanceTests.Pages.ApiKeys;

public class AddPage(IPage page) : PageLayout(page)
{
    private static string NameField => "Name";
    private static string SubmitButton => "SubmitButton";

    public static async Task<AddPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("/user/apikeys/add");
        return new AddPage(page);
    }

    public async Task EnterName(string name) =>
        await Page.GetByTestId(NameField).FillAsync(name);

    public async Task ClickSubmit() =>
        await Page
            .GetByTestId(SubmitButton)
            .ClickAsync();
}