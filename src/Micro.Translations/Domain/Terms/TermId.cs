namespace Micro.Translations.Domain.Terms;

public record TermId(Guid Value)
{
    public static TermId Create()=> new TermId(Guid.NewGuid());
    public static implicit operator string(TermId d) => d.Value.ToString();
};