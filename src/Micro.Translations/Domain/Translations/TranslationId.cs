namespace Micro.Translations.Domain.Translations;

public record TranslationId(Guid Value)
{
    public static TranslationId Create()=> new TranslationId(Guid.NewGuid());
    public static implicit operator string(TranslationId d) => d.Value.ToString();
};