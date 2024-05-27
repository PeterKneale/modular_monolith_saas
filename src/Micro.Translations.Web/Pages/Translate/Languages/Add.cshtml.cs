namespace Micro.Translations.Web.Pages.Translate.Languages;

public class Add(ITranslationModule module, IPageContextAccessor context) : PageModel
{
    [Display(Name = "Language Code")]
    [Required]
    [BindProperty]
    [StringLength(50)]
    public string LanguageCode { get; set; }

    public List<SelectListItem> Langauges { get; set; }

    public async Task<IActionResult> OnGetAsync() => await PopulateLanguages();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateLanguages();
            return Page();
        }

        try
        {
            await module.SendCommand(new AddLanguage.Command(Guid.NewGuid(), LanguageCode));
            TempData.SetAlert(Alert.Success("You have added a new language"));
            return RedirectToPage(nameof(Index), new
            {
                Org = context.Organisation.Name,
                Project = context.Project.Name
            });
        }
        catch (PlatformException e)
        {
            await PopulateLanguages();
            ModelState.AddModelError(string.Empty, e.Message!);
            return Page();
        }
    }

    private async Task<IActionResult> PopulateLanguages()
    {
        var languages = await module.SendQuery(new ListAllLanguages.Query());
        Langauges = languages.Select(language => new SelectListItem
        {
            Value = language.Code,
            Text = language.Name
        }).ToList();
        return Page();
    }
}