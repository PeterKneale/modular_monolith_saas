namespace Micro.Translations.Domain.LanguageAggregate;

public record LanguageId(Guid Value)
{
    public static LanguageId Create() => new(Guid.NewGuid());
    public static LanguageId Create(Guid guid) => new(guid);
    public static implicit operator Guid(LanguageId d) => d.Value;
}