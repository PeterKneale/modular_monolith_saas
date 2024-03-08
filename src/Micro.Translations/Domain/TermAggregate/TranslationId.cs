namespace Micro.Translations.Domain.TermAggregate;

public record TranslationId(Guid Value)
{
    public static TranslationId Create()=> new(Guid.NewGuid());
    public static implicit operator string(TranslationId d) => d.Value.ToString();
};