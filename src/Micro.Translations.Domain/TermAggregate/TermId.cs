namespace Micro.Translations.Domain.TermAggregate;

public class TermId : IdValueObject
{
    private TermId(Guid value):base(value)
    {
    }

    public static TermId Create() => new(Guid.NewGuid());
    public static TermId Create(Guid id) => new(id);

    public static implicit operator string(TermId d) => d.Value.ToString();
    public static implicit operator Guid(TermId d) => d.Value;
}