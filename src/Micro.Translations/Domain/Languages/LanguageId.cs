namespace Micro.Translations.Domain.Languages;

public record LanguageId(Guid Value)
{
    public static LanguageId Create()=> new LanguageId(Guid.NewGuid());
    public static implicit operator string(LanguageId d) => d.Value.ToString();
}