﻿using Micro.Web.AcceptanceTests.Pages.Auth;
using Micro.Web.AcceptanceTests.Pages.Organisations;
using Micro.Web.AcceptanceTests.Pages.Projects;
using Micro.Web.AcceptanceTests.UseCases;

namespace Micro.Web.AcceptanceTests.Extensions;

public static class PageExtensions
{
    public static async Task GotoRelativeUrlAsync(this IPage page, string url)
    {
        var root = Instance.BaseUrl;
        var uri = new Uri(root, url);
        await page.GotoAsync(uri.ToString());
    }

    public static async Task GivenLoggedIn(this IPage page)
    {
        var data = TestUser.CreateValid();

        var register = await RegisterPage.Goto(page);

        await register.Register(data.FirstName, data.LastName, data.Email, data.Password);
        await register.Alert.AssertSuccess();

        var userId = await page.GetUserId(data.Email);
        var token = await page.GetUserVerificationToken(userId);

        var verify = await VerifyEmailPage.Goto(page, userId, token);
        await verify.Alert.AssertSuccess();

        var login = await LoginPage.Goto(page);
        await login.Login(data.Email, data.Password);
        await login.Alert.AssertSuccess();
    }

    public static async Task<string> GivenAnOrganisationOwned(this IPage page) =>
        (await GivenANumberOfOrganisationOwned(page, 1)).Single();

    public static async Task<List<string>> GivenANumberOfOrganisationOwned(this IPage page, int count)
    {
        var list = new List<string>();
        for (var i = 0; i < count; i++)
        {
            var data = OrganisationCreatePageData.CreateValid();
            var create = await OrganisationCreatePage.Goto(page);
            await create.Create(data.Name);
            await create.Alert.AssertSuccess();
            list.Add(data.Name);
        }

        return list;
    }

    public static async Task<string> GivenAProjectOwned(this IPage page, string organisation) =>
        (await page.GivenProjectsOwned(organisation, 1)).Single();

    public static async Task<List<string>> GivenProjectsOwned(this IPage page, string organisation, int count)
    {
        var list = new List<string>();
        for (var i = 0; i < count; i++)
        {
            var data = ProjectCreatePageData.CreateValid();
            var create = await ProjectCreatePage.Goto(page, organisation);
            await create.Create(data.Name);
            await create.Alert.AssertSuccess();
            list.Add(data.Name);
        }

        return list;
    }
}