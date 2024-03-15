using Micro.AcceptanceTests.Pages.Layouts;

namespace Micro.AcceptanceTests.Pages.Auth;

public class RegisterPage(IPage page) : PageLayout(page)
{
    private static string FirstNameField => "FirstName";
    private static string LastNameField => "LastName";
    private static string EmailField => "Email";
    private static string PasswordField => "Password";
    private static string RegisterButton => "Register";

    public static async Task<RegisterPage> Goto(IPage page)
    {
        await page.GotoRelativeUrlAsync("Auth/Register");
        return new RegisterPage(page);
    }

    public async Task Register(string firstName, string lastName, string email, string password)
    {
        await Page.GetByTestId(FirstNameField).FillAsync(firstName);
        await Page.GetByTestId(LastNameField).FillAsync(lastName);
        await Page.GetByTestId(EmailField).FillAsync(email);
        await Page.GetByTestId(PasswordField).FillAsync(password);
        await Page.GetByTestId(RegisterButton).ClickAsync();
    }
}