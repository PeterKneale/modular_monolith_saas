namespace Micro.Web.Pages.Shared.Components.Menu;

public class Model
{
    public Guid UserId { get; init; }
    public string Email { get; init; }

    public IList<(Guid Id, string Name, bool IsCurrent)> Organisations { get; set; }
    
    public IList<(Guid Id, string Name, bool IsCurrent)> Projects { get; set; }
}