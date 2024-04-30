namespace Micro.Translations.Domain.TermAggregate;

public class TranslationId : IdValueObject
{
    private TranslationId(Guid value):base(value)
    {
    }

    public static TranslationId Create() => new(Guid.NewGuid());
    public static TranslationId Create(Guid id) => new(id);

    public static implicit operator string(TranslationId d) => d.Value.ToString();
    public static implicit operator Guid(TranslationId d) => d.Value;
}