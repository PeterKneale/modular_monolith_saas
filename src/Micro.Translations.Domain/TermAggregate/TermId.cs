namespace Micro.Translations.Domain.TermAggregate;

public record TermId(Guid Value)
{
    public static TermId Create() => new(Guid.NewGuid());
    public static TermId Create(Guid guid) => new(guid);
    public static implicit operator Guid(TermId d) => d.Value;
}