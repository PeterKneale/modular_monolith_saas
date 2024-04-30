namespace Micro.Translations.Domain.LanguageAggregate;

public class LanguageId : IdValueObject
{
    private LanguageId(Guid value):base(value)
    {
    }

    public static LanguageId Create() => new(Guid.NewGuid());
    public static LanguageId Create(Guid id) => new(id);

    public static implicit operator string(LanguageId d) => d.Value.ToString();
    public static implicit operator Guid(LanguageId d) => d.Value;
}