namespace Micro.Web.AcceptanceTests.Pages.Auth;

public class RegisterPage(IPage page)
{
    private static string FirstNameField => "FirstName";
    private static string LastNameField => "LastName";
    private static string EmailField => "Email";
    private static string PasswordField => "Password";
    private static string RegisterButton => "Register";

    public static RegisterPage Goto(IPage page)
    {
        page.GotoAsync(Constants.BaseUrl + "Auth/Register");
        return new RegisterPage(page);
    }

    public async Task Register(string firstName, string lastName, string email, string password)
    {
        await page.GetByTestId(FirstNameField).FillAsync(firstName);
        await page.GetByTestId(LastNameField).FillAsync(lastName);
        await page.GetByTestId(EmailField).FillAsync(email);
        await page.GetByTestId(PasswordField).FillAsync(password);
        await page.GetByTestId(RegisterButton).ClickAsync();
    }
}