namespace Micro.Translations.Domain.TermAggregate;

public record TranslationId(Guid Value)
{
    public static TranslationId Create() => new(Guid.NewGuid());
    public static TranslationId Create(Guid guid) => new(guid);
    public static implicit operator Guid(TranslationId d) => d.Value;
}